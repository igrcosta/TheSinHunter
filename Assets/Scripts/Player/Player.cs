/* using Unity.VisualScripting; */
using UnityEngine;
/* using System.Collections;
using System;
using NUnit.Framework; */

public class Player : MonoBehaviour
{
    [Header("Sistema De Corrida")]
    public float Speed = 1.5f;
    [SerializeField] float MultiplicadorVelocidade = 20f;
    private bool canMove = true;     //variavel para poder parar o movimento do player quando quiser

    [Header("Infos para Pulo")]
    [SerializeField] float JumpForce = 10f;
    public bool CanJump = false;
    private bool OnJump = false;
    private bool isparrying = false;

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

    //variavel para combo de avarezas (MELHORA FEELING)
    //public bool isOnA

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
        if (isDashing)
        {
            rb.useGravity = false;
            return;
        }
        //lógica de física melhorada aqui, isso vai permitir uma queda irada pro player
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        rb.useGravity = true;
    }

    void Update()
    {
        CanDash();
        Move();
        SpeedSystem();
        Jumping();
        DescendingLanes();
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

        if (rb.position.y - JumpingBeginning >= 10f && OnJump && !isparrying)
        {
            //rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.down * JumpForce / 55f, ForceMode.Impulse);
        }

        if (ActualWeapon == WeaponTypes.Default && rb.position.x - DashingBeginning >= 30f && isDashing)
        {
            rb.linearVelocity = new Vector3(0, 0, rb.linearVelocity.z);
            //rb.AddForce(Vector3.down * JumpForce / 55f, ForceMode.Impulse);
        }


        //parte das correntes INÍCIO
        if (ActualWeapon == WeaponTypes.LuxuryChains && isDashing && TargetObject != null)
        {
            DisableChainsLayersCollision();
            rb.position = Vector3.MoveTowards(rb.position, TargetObject.transform.position, dashingpower * Time.deltaTime);

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

        //MultiplicadorVelocidade -= 2f;
        //reduz a taxa de regeneração

        DamageInvulnerability = false;
        //volta a incrementar velocidade
    }

    void DeathCondition()
    {
        if (GameController.controller.Cheating)
        {
            //nada
        }
        else if (Speed <= 20 && GameController.controller.Cheating == false)
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
    #endregion Speed/Damage Logic

    #region Jumping
    //Pulo do jogador -> INÍCIO

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Floor"))
        //lembrar que com colisões, precisamos acessar o gameObject deles para pegar coisas como tags
        {
            CanJump = true;
            OnJump = false;
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
            OnJump = false;
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

    //LavaKill também é utilizado para quando se bate em onstáculos não unlocked ainda
    public void LavaKill()
    {
        //player para
        canMove = false;

        //player começa a descer
        Invoke("DisableLayersCollision", 0.25f);

        GameController.controller.Invoke("GameOver", 0.8f);
        //invoca depois de alguns segundos a tela de morte
    }
    void Jumping()
    {
        //script para o player poder pular
        //ao clicar na seta pra cima, ele define a força y dele pra 0, para poder dar um impulso pra cima
        //as outras forças ele mantém padrão, mantendo o X como deveria estar
        //JÁ FUNCIONA ATÉ PARA PULO DUPLO

        if (Input.GetKeyDown(KeyCode.UpArrow) && CanJump)
        {
            OnJump = true;

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

    public void MobileJumping()
    {
        if (CanJump)
        {
            OnJump = true;

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

    //Pulo do jogador -> FINAL

    #endregion Jumping

    #region Lanes
    //Troca de Lanes -> INÍCIO
    void DescendingLanes()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && !IsOnALane && !CanJump)
        {
            rb.AddForce(Vector3.down * JumpForce / 2f, ForceMode.VelocityChange);
        }
        else
            if (Input.GetKeyDown(KeyCode.DownArrow) && IsOnALane)
            {
                DisableLayersCollision();
                rb.AddForce(Vector3.down * JumpForce / 3f, ForceMode.VelocityChange);
            }
        //primeiro ao apertar seta pra baixo



        //depois pra quando pular e esbarrar em algo da tag Lane
    }

    void EnableLayersCollision()
    {
        Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, false);
        rb.AddForce(Vector3.down * JumpForce / 35f, ForceMode.Impulse);
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

    public void MobileDescendingLanes()
    {
        if (!IsOnALane && !CanJump)
        {
            rb.AddForce(Vector3.down * JumpForce / 2f, ForceMode.VelocityChange);
        }
        else
            if (IsOnALane)
            {
                DisableLayersCollision();
                rb.AddForce(Vector3.down * JumpForce / 3f, ForceMode.VelocityChange);
            }
        //primeiro ao apertar seta pra baixo



        //depois pra quando pular e esbarrar em algo da tag Lane
    }

    //Troca de Lanes -> FINAL
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
        //limitar de vel para explosões INÍCIO
        if (ActualWeapon == WeaponTypes.RageBlade)
        {
            if (rb.position.y >= 39f)
            {
                rb.MovePosition(new Vector3(rb.position.x, rb.position.y - 10f, rb.position.z));
            }
            if (rb.linearVelocity.x >= 90)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x - rb.linearVelocity.x / 4f, rb.linearVelocity.y, rb.linearVelocity.z);
            }
            /* if (rb.linearVelocity.y >= 40)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y - rb.linearVelocity.y / 4f, rb.linearVelocity.z);
            } */
        }
        //limitar de vel para explosões FIM
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

    #region Dashes
    public void BeginDash()
    {
        //O DASH MUDA CONFORME FOR A ARMA UTILIZADA, LOGO...
        //caso seja um dash default...
        if (ActualWeapon == WeaponTypes.Default)
        {
            canDash = false;
            //permitimos isso para cooldown começar a rodar

            Debug.Log("ARMA DEFAULT EQUIPADA");
            isDashing = true;
            tr.enabled = true;

            DashingBeginning = rb.position.x;
            rb.linearVelocity = Vector3.zero;

            rb.AddForce(Vector3.right * dashingpower, ForceMode.Impulse);

            //Speed -= Speed/10;

            //rb.MovePosition(rb.position + Vector3.right * dashingpower);
            Invoke("FinishDash", dashingtime);
        }
        //Se não, se a arma utilizada for as correntes...
        else if (ActualWeapon == WeaponTypes.LuxuryChains)
        {
            Debug.Log("LUXURY CHAINS utilizada");
            isDashing = true;
            tr.enabled = true;

            //"lerp" para a direção do alvo

        }
        //se não, se a arma atual for a lâmina da ira...
        else if (ActualWeapon == WeaponTypes.RageBlade)
        {
            canDash = false;
            DamageInvulnerability = true;

            //instanciar explosão
            Instantiate(ExplosionPrefab, BombSpawn.transform.position, BombSpawn.transform.rotation);

            //lançar para frente
            RageExplosion();

        }

    }

    public void FinishDash()
    {
        if (ActualWeapon == WeaponTypes.LuxuryChains)
        {
            Vector3 ParryEffect = new Vector3(0f, 9.81f, 0f);

            rb.AddForce(ParryEffect * 7f, ForceMode.Impulse);

            INSTAEnableLayersCollision();

            ChainsScript.SelectNewTarget();

            //procurar outro
        }
        //rb.linearVelocity = Vector3.zero;
        tr.enabled = false;
        isDashing = false;
        //canDash = true;
    }

    void CanDash() // Checa se pode dar dash, e roda se possivel
    {
        if (ActualWeapon == WeaponTypes.Default) //depois colocar a lamina tbm
        {
            if (!canDash)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                BeginDash();
            }

        }
        else if (ActualWeapon == WeaponTypes.LuxuryChains)
        {
            if (!canDash)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && ChainsScript.ActualTarget != null)
            {
                BeginDash();
            }
        }
        else if (ActualWeapon == WeaponTypes.RageBlade)
        {
            if (!canDash)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                BeginDash();
                ExplosionState = true;
            }
        }
    }

    public void EnableDash()
    {
        //método para permitir o parry poder habilitar mais dashes ao jogador
        CancelInvoke("FinishDash");
        rb.linearVelocity = Vector3.zero;

        isDashing = false;
        tr.enabled = false;
        canDash = true;
    }

    public void ComboDash()
    {
        //a ideia aqui é fazer o player continuar no dash após ter matado avareza
        CancelInvoke("FinishDash");

        BeginDash();
    }
    #endregion Dashes

    public void ParryLogicEnable()
    {
        //a ideia é ignorar o dash da sua lógica padrão ao dar parry
        isparrying = true;
    }
}