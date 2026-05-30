
using System.Collections;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Sistema De Corrida")]
    public float Speed = 1.5f;
    [SerializeField] float MultiplicadorVelocidade = 20f;
    private bool canMove = true;     //variavel para poder parar o movimento do player quando quiser

    [Header("Pulo")]
    [SerializeField] float JumpForce = 10f;
    public float velocidade, JumpTimer, JumpingDuration = 1, DelayGravidadePulo, PotenciaGravity;
    public AnimationCurve jumpCurve;
    public bool CanJump = false;
    private bool Jumping = false;
    private Coroutine jumpCoroutine;
    private bool cancelJumpRequested = false;
    public bool isparrying = false;



    [SerializeField] bool DoubleJumpUnlocked = true;
    public bool CanDoubleJump = false;
    public bool IsDoubleJumping = false;


    private Vector3 ParryEffect = new Vector3(0f, 3f, 0f);

    [Header("Dashs")]
    public bool isDashing = false;
    [SerializeField] bool canDash = true;
    [SerializeField] float dashingpower = 100;
    [SerializeField] float dashingCD = 1f;
    [SerializeField] float dashingtime = 1f;
    [SerializeField] bool DoubleDashUnlocked = true;
    private float DefaultdashCD;
    public float DashingBeginning;
    private Vector3 dashTargetPosition;

    public bool SuperDashEnable = false;

    //variáveis para controlar gravidade
    [Header("GRAVIDADE")]
    [SerializeField] float gravityScale = 5f;
    [SerializeField] float fallingGravityScale = 30f;
    private float currentGravityScale;
    bool ApplyGravity = true;

    [SerializeField] GameObject SkySlash;


    [Header("Armas/Mecânicas")]
    public WeaponTypes ActualWeapon;
    private GameObject ChainsTriggerRef;
    private ChainsScript ChainsScript;
    public enum WeaponTypes { Default, LuxuryChains, RageBlade };
    [SerializeField] ExplosionScript ExplosionPrefab;
    public bool ExplosionState = false;
    private Vector3 ExplosionForce = new Vector3(0f, 70f, 0f);
    private Vector3 SecondExplosionForce = new Vector3(9f, 4f, 0f);


    [Header("Referencias")] //Referencias e Variaveis
    private GameObject BombSpawn;
    bool DamageInvulnerability = false;
    public TrailRenderer tr; //Trilha Dash
    private Vector3 V3Move;
    private Vector3 JumpVector;
    private Vector3 Target;
    public GameObject JumpLocation;
    public GameObject DashLocation;
    private bool DummyMode = false;
    public bool ChainsActive = false; //MODO: Chains
    public bool DefaultActive = false; //MODO: Default
    public bool RageActive = false; //MODO: Rage
    public GameObject TargetObject; // Target Chains
    public Rigidbody rb;
    float currentPosition = 0;

    public bool SlashActive = false;
    public Transform ShootPoint;

    //variavel para combo de avarezas (MELHORA FEELING)

    //variáveis para lanes
    private BoxCollider LaneCollider;
    private bool IsOnALane = false;
    private int LanesLayer;
    private int PlayerLayer;
    private float JumpingBeginning;
    [SerializeField] GameObject outlineObj;


    void Awake()
    {
        GameController.controller.playerRef = this; //Referencia Player

        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        //Encontrar referência Do trigger das correntes
        ChainsTriggerRef = transform.GetChild(1).gameObject;

        ChainsScript = ChainsTriggerRef.GetComponent<ChainsScript>();

        //parte envolvendo scrips da rageBlade INÍCIO
        BombSpawn = transform.GetChild(3).gameObject;
        //parte envolvendo scrips da rageBlade FIM


        //reset para caso começe o jogo com arma X, aparecer o que deveria para sua arma
        WeaponChecking();

        //identificar qual camada de colisão é qual para permitir atravessar as lanes
        LanesLayer = LayerMask.NameToLayer("Lanes");
        PlayerLayer = LayerMask.NameToLayer("Player");

        //gravidadezinha marota e lógica pra permitir o player cair mais rápido, tipo no mário
        currentGravityScale = gravityScale;

        if (rb.linearVelocity.y > 0)
        {
            currentGravityScale = gravityScale;
        }
        else if (rb.linearVelocity.y < 0)
        {
            currentGravityScale = fallingGravityScale;
        }

        canMove = true;

        DefaultdashCD = dashingCD;
    }

    #region Updates
    void FixedUpdate()
    {
        if (ApplyGravity) ApplyExtraGravity();

    }

    void Update()
    {
        Move();
        SpeedSystem();
        DeathCondition();
        Dashtimer();

        if(ActualWeapon == WeaponTypes.Default && isDashing)
        {
            //deixa ele mexer apenas em x
            Vector3 next = Vector3.MoveTowards(rb.position, dashTargetPosition, dashingpower * Time.deltaTime);
            rb.MovePosition(next);

            //ele para o dash gando chega na posicao salva
            if (Vector3.Distance(rb.position, dashTargetPosition) < 0.1f)
            {
                FinishDash();
            }
        }


        //parte das correntes INÍCIO
        if (ActualWeapon == WeaponTypes.LuxuryChains && isDashing)
        {
            // SEGUNDA PROTEÇÃO: Se o dash está ativo mas o alvo sumiu do mapa
            if (TargetObject == null)
            {
                FinishDash();
            }
            else
            {
                Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, true);

                rb.MovePosition ( Vector3.MoveTowards(rb.position, TargetObject.transform.position, dashingpower * Time.deltaTime * 1.5f ));

                // CHECAGEM DE CHEGADA: Se estiver muito perto do alvo, encerra o dash
                if (Vector3.Distance(transform.position, TargetObject.transform.position) < 0.5f)
                {
                    FinishDash();
                }
            }
        }
        //parte das correntes FIM
        RageSpeedLimiter();
    }

    #endregion Updates


   

    #region Speed/Damage Logic

    void SpeedSystem() // Sistema de aumento de velocidade
    {
        if (Speed <= 30 && !DummyMode)
        {
            if (!DamageInvulnerability)
            {
                Speed += MultiplicadorVelocidade * Time.deltaTime;
            }
        }
    }

    //função para espinhos detectarem colisão
    public void Hit(float damage)
    {
        DamageInvulnerability = true;
        //detecta colisão para parar de incrementar a velocidade

        Speed -= damage;
        //reduz a speed com base na vida

        //reduz a taxa de regeneração

        DamageInvulnerability = false;
        //volta a incrementar velocidade
        if(!isDashing)
        HitEffect();
    }
    void HitEffect()
    {
        
        outlineObj.SetActive(true);
    }

    void DeathCondition()
    {
        if (GameController.controller.Cheating)
            return;
        else if (Speed <= 20)
        {
            GameController.controller.GameOver();
        }
    }

    void Move() // Sistema de Corrida infinita
    {
        if (canMove && !isDashing)
        {
            //rb.position += Vector3.right * Speed * Time.deltaTime
            rb.MovePosition(rb.position + Vector3.right * Speed * Time.deltaTime * 2);
        }
    }

    void ApplyExtraGravity()
    {
        if (isDashing && !Jumping)
        {
            rb.useGravity = false;
            return;
        }
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        rb.useGravity = true;

        if (rb.position.y - JumpingBeginning >= 10f && Jumping && !isparrying && !ExplosionState)
        {
            rb.AddForce(Vector3.down * JumpForce / 55f, ForceMode.Impulse);
        }
    }
    #endregion Speed/Damage Logic

    #region Jumping
    void OnCollisionEnter(Collision collisionInfo)
    {

        if (collisionInfo.gameObject.CompareTag("Floor"))
        {
            CanJump = true;
            Jumping = false;
            isparrying = false;
            IsDoubleJumping = false;
            CanDoubleJump = true;



            if (ActualWeapon == WeaponTypes.RageBlade)
            {
                ExplosionState = false;
                DamageInvulnerability = false;
            }
        }
        

        //PARTE PARA DESCER DE LANES
        else if (collisionInfo.gameObject.CompareTag("Lane"))
        {
            CanJump = true;
            CanDoubleJump = true;
            IsDoubleJumping = false;
            IsOnALane = true;
            Jumping = false;
            isparrying = false;

            if (ActualWeapon == WeaponTypes.RageBlade)
            {
                ExplosionState = false;
                DamageInvulnerability = false;
            }
            if(outlineObj.activeSelf)
            {
                outlineObj.SetActive(false);
            }
        }

        else if (collisionInfo.gameObject.CompareTag("BrittleWall"))
        {
            CanJump = false;
            CanDoubleJump = true;
            IsDoubleJumping = false;
            IsOnALane = false;
            Jumping = false;
            isparrying = false;

            if (ActualWeapon == WeaponTypes.RageBlade)
            {
                ExplosionState = false;
                DamageInvulnerability = false;
            }
            if (outlineObj.activeSelf)
            {
                outlineObj.SetActive(false);
            }
        }

        //Parte da Lava / obstáculos 
        else if (collisionInfo.gameObject.CompareTag("LAVA"))
        {
            FinishDash();
            LavaKill();
        }

        else if (collisionInfo.gameObject.CompareTag("Wall"))
        {
            FinishDash();
            LavaKill();
        }

        else if (collisionInfo.gameObject.CompareTag("BrittleWall"))
        {
            if (!SuperDashEnable)
            {
                FinishDash();
                LavaKill();
            }
            else
            {
                EnableDash();
            }
        }



        else
        {
            CanJump = false;
        }
        
    }

    

    //LavaKill também é utilizado para quando se bate em obstáculos não unlocked ainda
    public void LavaKill()
    {
        canMove = false;
        Invoke("DisableLayersCollision", 0.25f);
        GameController.controller.Invoke("GameOver", 0.4f);
        //invoca depois de alguns segundos a tela de morte
    }


    public void JumpingMethod()
    {
        if (isDashing || IsDoubleJumping) return;
        if (CanJump && !isparrying)
        {
            cancelJumpRequested = false;
            Jumping = true;
            CanJump = false;
            ApplyGravity = false;
            rb.useGravity = false;
            JumpTimer = 0;

            float alturaAtual = rb.position.y;
            float AlturaAlvo = JumpLocation.transform.position.y;

            Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, true);

            jumpCoroutine = StartCoroutine(JumpCoroutine(alturaAtual, AlturaAlvo));
        }
    }
    
    IEnumerator JumpCoroutine(float inicial,float alvo)
    {
       while (JumpTimer < JumpingDuration) // Pulo
        {
            yield return null;

            if (cancelJumpRequested)
                yield break;

            JumpTimer += Time.deltaTime; //Roda o timer por segundo

            float progresso = JumpTimer / JumpingDuration; // Calcula porcentagem do progresso do pulo

            float CurvaDeTempo = jumpCurve.Evaluate(progresso); //Define o progresso como uma curva

            float newbaby = Mathf.Lerp(inicial,alvo, CurvaDeTempo); //Faz um lerp da distancia aonde deve ir e a atual com a curva feita no progresso

            //rb.position = new Vector3(rb.position.x, newbaby, rb.position.z); 

            rb.position = new Vector3(rb.position.x, newbaby, rb.position.z); //Move

        
        }

        float Timernoar = 0;
        float Delay = DelayGravidadePulo;

       while (Timernoar < Delay) // Gravidade do meio termo
        {
            if (isDashing || isparrying)
                break;

            if (cancelJumpRequested)
                yield break;

            Timernoar += Time.deltaTime;

            rb.MovePosition(rb.position + Vector3.down * PotenciaGravity * Time.deltaTime);

            yield return null;
        }

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        Jumping = false;
        Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, false);

        CanDoubleJump = true;
        ApplyGravity = true;
        rb.useGravity = true;
    }

    void CancelJump()
    {
        //cancelJumpRequested = true;
        //JumpTimer = JumpingDuration;

        if (jumpCoroutine != null)
        {
           
            StopCoroutine(jumpCoroutine);
            jumpCoroutine = null;
        }
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        Jumping = false;
        ApplyGravity = true;
        rb.useGravity = true;
            Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, true);
    }

    public void DoubleJump()
    {
        if(!CanJump && CanDoubleJump && DoubleJumpUnlocked)
        {

            CancelJump();
            FinishDash();

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, 0f);
            rb.useGravity = true;
            ApplyGravity = true;

     
            IsDoubleJumping = true;
            RageExplosion();
            CanDoubleJump = false;
            if (DoubleJumpUnlocked) canDash = true;
        }
    }

    #endregion Jumping

    #region Lanes
    public void DescendingLanes()
    {
        if (isDashing) return;
        if (!IsOnALane && !CanJump)
        {
            CancelJump();
            rb.AddForce(Vector3.down * JumpForce, ForceMode.VelocityChange);
       
        }
        else if (IsOnALane)
        {
            DisableLayersCollision();
            rb.AddForce(Vector3.down * JumpForce / 3f, ForceMode.VelocityChange);
        }
    }

    void EnableLayersCollision()
    {
        Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, false);
    }

    void DisableLayersCollision()
    {
        Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, true);
        Invoke("EnableLayersCollision", 0.1f);
    }

    void DisableChainsLayersCollision()
    {
        Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, true);
        Debug.Log("IGNORADO");
    }

    void INSTAEnableLayersCollision()
    {
        Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, false);
        Debug.Log("LEMBREI DE VC HEHE");
    }
    #endregion Lanes

    #region Weapons
    void WeaponChecking()
    {
        if (ActualWeapon == WeaponTypes.Default)
        {
            //desligar tudo o que não for preciso para arma default

            GameController.controller.UIManager.DisableAim();
            //desabilitar mira de corrente

            ChainsActive = false;
            ChainsTriggerRef.SetActive(false);
            DefaultActive = true;
            RageActive = false;
        }
        else if (ActualWeapon == WeaponTypes.LuxuryChains)
        {
            ChainsActive = true;
            ChainsTriggerRef.SetActive(true);
            DefaultActive = false;
            RageActive = false;
        }
        else if (ActualWeapon == WeaponTypes.RageBlade)
        {
            GameController.controller.UIManager.DisableAim();
            //desabilitar mira de corrente

            ChainsActive = false;
            ChainsTriggerRef.SetActive(false);
            DefaultActive = false;
            RageActive = true;
        }
    }

    public void SetActualWeapon(string weaponName)
    {
        if (weaponName == "LuxuryChains")
        {
            ActualWeapon = WeaponTypes.LuxuryChains;
        }
        else if (weaponName == "Default")
        {
            ActualWeapon = WeaponTypes.Default;
        }
        else if (weaponName == "RageBlade")
        {
            ActualWeapon = WeaponTypes.RageBlade;
        }
        WeaponChecking();
    }

    #endregion WeaponChecking

    #region RageMethods

    private void RageSpeedLimiter()
    {
        if (ActualWeapon == WeaponTypes.RageBlade)
        {
            if (rb.linearVelocity.x >= 40)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x - rb.linearVelocity.x / 4f, rb.linearVelocity.y, rb.linearVelocity.z);
            }
        }
    }
    public void RageExplosion()
    {
       
        rb.AddForce(ExplosionForce, ForceMode.Impulse);
        DisableChainsLayersCollision();
        Invoke("INSTAEnableLayersCollision", 0.5f);
 
    }

    public void ContinuousRageExplosion()
    {
        CancelInvoke("EnableLayersCollision");

        rb.AddForce(ParryEffect * 7f, ForceMode.Impulse);


        DisableLayersCollision();
        Invoke("INSTAEnableLayersCollision", 0.5f);

    }
    #endregion RageMethods


    public void BeginSlash()
    {
        SlashActive = true;
        Instantiate(SkySlash, ShootPoint.position, transform.rotation);
    }


    #region Dashes

    public void Dashtimer()
    {
        if (!canDash)
        {
            dashingCD -= Time.deltaTime;
            if (dashingCD <= 0)
            {
                canDash = true;
                dashingCD = DefaultdashCD;
            }
        }
    }
    public void BeginDash()
    {
        // Se já estiver dando dash, ignora qualquer novo comando de dash
        if (isDashing || !canDash) return;

        CancelJump();

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, 0f);

        ApplyGravity = true;
        rb.useGravity = true;
        Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, false);

        switch (ActualWeapon)
        {
            case WeaponTypes.Default:
                {
                    canDash = false;
                    isDashing = true;
                    tr.enabled = true;
                    DashingBeginning = rb.position.x;
                    Debug.Log("ARMA DEFAULT EQUIPADA");

                    dashTargetPosition = DashLocation.transform.position;
                    // isso salva a posicao do dash quando vc clicou no botao, para a posicao do dash nao ficar andando junto com ele

                    break;
                }
            case WeaponTypes.LuxuryChains:
                {
                    if (ChainsScript.ActualTarget == null)
                        return;
                    canDash = false;
                    isDashing = true;
                    tr.enabled = true;
                    Debug.Log("LUXURY CHAINS utilizada");
                    break;
                }
            case WeaponTypes.RageBlade:
                {
                    ExplosionState = true;
                    canDash = false;
                    DamageInvulnerability = true;

                    Instantiate(ExplosionPrefab, BombSpawn.transform.position, BombSpawn.transform.rotation);

                    RageExplosion();//lançar para frente

                    break;
                }
        }
    }


    public void FinishDash()
    {
        ApplyGravity = true;
        rb.useGravity = true;
        Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, false);

        if (ActualWeapon == WeaponTypes.LuxuryChains)
        {
            INSTAEnableLayersCollision();

            Vector3 ParryEffect = new Vector3(0f, 9.81f, 0f);

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            //resetamos a velocidade pra não acumular nada vertical

            rb.AddForce(ParryEffect * 7f, ForceMode.Impulse);

            

            ChainsScript.SelectNewTarget();
        }

        if(ActualWeapon == WeaponTypes.Default)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, rb.linearVelocity.z);
        }

        rb.useGravity = true;
        tr.enabled = false;
        isDashing = false;
    }

    public void EnableDash()
    {


        //método para permitir o parry poder habilitar mais dashes ao jogador
        CancelInvoke("FinishDash");
        rb.linearVelocity = Vector3.zero;
        //rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0);

        if (ActualWeapon == WeaponTypes.LuxuryChains)
        INSTAEnableLayersCollision();


        isDashing = false;
        tr.enabled = false;
        canDash = true;

    }

    public void ComboDash()
    {

        CancelInvoke("FinishDash");

        if(ActualWeapon == WeaponTypes.LuxuryChains)
        INSTAEnableLayersCollision();

        isDashing = false;
        tr.enabled = false;
        canDash = true;

        EnableDash();
    }
    #endregion Dashes

   
}