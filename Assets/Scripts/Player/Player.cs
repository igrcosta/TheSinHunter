
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Sistema De Corrida")]
    public float Speed = 1.5f;
    [SerializeField] float MultiplicadorVelocidade = 20f;
    private bool canMove = true;     //variavel para poder parar o movimento do player quando quiser

    [Header("Infos para Pulo")]
    [SerializeField] float JumpForce = 10f;
    public bool CanJump = false;
    private bool Jumping = false;
    public bool isparrying = false;

    private Vector3 ParryEffect = new Vector3(0f, 9.81f, 0f);

    [Header("Dashs")]
    public bool isDashing = false;
    [SerializeField] bool canDash = true;
    [SerializeField] float dashingpower = 100;
    [SerializeField] float dashingCD = 1f;
    [SerializeField] float dashingtime = 1f;
    private float DefaultdashCD;
    public float DashingBeginning;

    //variáveis para controlar gravidade
    [Header("GRAVIDADE")]
    [SerializeField] float gravityScale = 5f;
    [SerializeField] float fallingGravityScale = 30f;
    private float currentGravityScale;



    [Header("Armas/Mecânicas")]
    public WeaponTypes ActualWeapon;
    private GameObject ChainsTriggerRef;
    private ChainsScript ChainsScript;
    public enum WeaponTypes { Default, LuxuryChains, RageBlade };
    [SerializeField] ExplosionScript ExplosionPrefab;
    public bool ExplosionState = false;
    private Vector3 ExplosionForce = new Vector3(9f, 2.8f, 0f) * 70f / 4.5f;
    private Vector3 SecondExplosionForce = new Vector3(9f, 4f, 0f);


    [Header("Referencias")] //Referencias e Variaveis
    private GameObject BombSpawn;
    bool DamageInvulnerability = false;
    private TrailRenderer tr; //Trilha Dash
    private Vector3 V3Move;
    private Vector3 JumpVector;
    private Vector3 Target;
    private bool DummyMode = false;
    public bool ChainsActive = false; //MODO: Chains
    public bool DefaultActive = false; //MODO: Default
    public bool RageActive = false; //MODO: Rage
    public GameObject TargetObject; // Target Chains
    public Rigidbody rb;
    float currentPosition = 0;

    //variavel para combo de avarezas (MELHORA FEELING)

    //variáveis para lanes
    private BoxCollider LaneCollider;
    private bool IsOnALane = false;
    private int LanesLayer;
    private int PlayerLayer;
    private float JumpingBeginning;

    void Awake()
    {
        GameController.controller.playerRef = this; //Referencia Player

        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
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
        ApplyExtraGravity();
    }

    void Update()
    {
        Move();
        SpeedSystem();
        DeathCondition();

        if (!canDash)
        {
            dashingCD -= Time.deltaTime;
            if (dashingCD <= 0)
            {
                canDash = true;
                dashingCD = DefaultdashCD;
            }
        }

        if (ActualWeapon == WeaponTypes.Default && rb.position.x - DashingBeginning >= 30f && isDashing)
        {
            rb.linearVelocity = new Vector3(0, 0, rb.linearVelocity.z);
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
                DisableChainsLayersCollision();
                rb.position = Vector3.MoveTowards(rb.position, TargetObject.transform.position, dashingpower * Time.deltaTime);

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
        if (Speed <= 50 && !DummyMode)
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
        if (canMove)
        {
            rb.position += Vector3.right * Speed * Time.deltaTime;
        }
    }

    void ApplyExtraGravity()
    {
        if (isDashing)
            return;
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

            if (ActualWeapon == WeaponTypes.RageBlade)
            {
                ExplosionState = false;
                DamageInvulnerability = false;
            }
        }
        else
        {
            CanJump = false;
        }

        //PARTE PARA DESCER DE LANES
        if (collisionInfo.gameObject.CompareTag("Lane"))
        {
            CanJump = true;
            IsOnALane = true;
            Jumping = false;
            isparrying = false;

            if (ActualWeapon == WeaponTypes.RageBlade)
            {
                ExplosionState = false;
                DamageInvulnerability = false;
            }
        }

        //Parte da Lava / obstáculos 
        if (collisionInfo.gameObject.CompareTag("LAVA"))
        {
            LavaKill();
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
        //script para o player poder pular
        //ao clicar na seta pra cima, ele define a força y dele pra 0, para poder dar um impulso pra cima
        //as outras forças ele mantém padrão, mantendo o X como deveria estar
        //JÁ FUNCIONA ATÉ PARA PULO DUPLO

        if (CanJump && !isparrying)
        {
            Jumping = true;

            JumpingBeginning = rb.position.y;
            //pego a posição do pulo para limitar a altura do pulo

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            //zero a velocidade em y 

            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            //aplicamos a força em Y como impulso, de forma que mantenha a velocidade de X */

            DisableLayersCollision();

            CanJump = false;

            //vou ter que sair da posição dele atual e subir 10f mantendo X e Z
        }
    }

    #endregion Jumping

    #region Lanes
    public void DescendingLanes()
    {
        if (!IsOnALane && !CanJump)
        {
            rb.AddForce(Vector3.down * JumpForce / 2f, ForceMode.VelocityChange);
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
        Invoke("EnableLayersCollision", 0.22f);
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
            if (rb.position.y >= 80)
            {
                rb.MovePosition(new Vector3(rb.position.x, rb.position.y - 10f, rb.position.z));
            }
            if (rb.linearVelocity.x >= 40)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x - rb.linearVelocity.x / 4f, rb.linearVelocity.y, rb.linearVelocity.z);
            }
        }
    }
    public void RageExplosion()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(ExplosionForce, ForceMode.Impulse);
        DisableChainsLayersCollision();
        Invoke("INSTAEnableLayersCollision", 0.5f);
    }

    public void ContinuousRageExplosion()
    {
        CancelInvoke("EnableLayersCollision");

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(ParryEffect * 7f, ForceMode.Impulse);


        DisableLayersCollision();
        Invoke("INSTAEnableLayersCollision", 0.5f);

    }
    #endregion RageMethods

    #region Dashes
    public void BeginDash()
    {
        // Se já estiver dando dash, ignora qualquer novo comando de dash
        if (isDashing || !canDash) return;

        switch (ActualWeapon)
        {
            case WeaponTypes.Default:
                {
                    canDash = false;
                    isDashing = true;
                    tr.enabled = true;
                    DashingBeginning = rb.position.x;
                    rb.linearVelocity = Vector3.zero;
                    rb.useGravity = false;
                    Debug.Log("ARMA DEFAULT EQUIPADA");

                    rb.AddForce(Vector3.right * dashingpower, ForceMode.Impulse);

                    Invoke("FinishDash", dashingtime);
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
        if (ActualWeapon == WeaponTypes.LuxuryChains)
        {
            INSTAEnableLayersCollision();

            Vector3 ParryEffect = new Vector3(0f, 9.81f, 0f);

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            //resetamos a velocidade pra não acumular nada vertical

            rb.AddForce(ParryEffect * 7f, ForceMode.Impulse);

            

            ChainsScript.SelectNewTarget();
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

        if(ActualWeapon == WeaponTypes.LuxuryChains)
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

        EnableDash();
    }
    #endregion Dashes

   
}