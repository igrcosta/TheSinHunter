using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{


    [Header("Sistema De Corrida")]
    [SerializeField] float Speed = 1.5f;
    [SerializeField] float LimiteVelocidade = 1.5f;

    [SerializeField] float MultiplicadorVelocidade = 20f;

    [Header("Postura")]
    [SerializeField] int Postura = 1;
    [SerializeField] float Taxaderegeneracao = 1;

    [Header("Infos para Pulo")]
    [SerializeField] float JumpForce = 10f;
    private Vector3 JumpVector;
    public bool CanJump = false;
    private bool OnJump = false;

    private bool TESTE = false;
    private Vector3 Target;
    private float y;

    [Header("Dashs")]
    [SerializeField] bool isDashing = false;
    [SerializeField] bool canDash = true;
    [SerializeField] float dashingpower = 100;
    [SerializeField] float dashingCD = 1f;
    [SerializeField] float dashingtime = 1f;

    private float DashingBeginning;

    //variáveis para controlar gravidade
    [Header("GRAVIDADE")]
    [SerializeField] float gravityScale = 5f;
    [SerializeField] float fallingGravityScale = 30f;
    private float currentGravityScale;



    //variáveis para lanes
    private BoxCollider LaneCollider;
    private bool IsOnALane = false;

    private int LanesLayer;
    private int PlayerLayer;
    private float JumpingBeginning;



    //Variaveis privadas
    float RegeneracaoPostura = 1;
    bool DamageInvulnerability = false;
    public Rigidbody rb;
    public TrailRenderer tr;
    public Animator mAnimator;
    private Vector3 V3Move;

    //variavel para poder parar o movimento do player quando quiser
    private bool canMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();

        GameController.controller.playerRef = this;
    }

    void Start()
    {
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
    }

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

        //(pelamor de Deus, rigidbody pra player é quase tentar ganhar uma triatlo sem saber nadar, tudo começa bem, mas no final...) 

    }

    void Update()
    {
        ChecagemDash();
        Mover();
        SistemaPostura();
        Jumping();
        DescendingLanes();

        if (rb.position.y - JumpingBeginning >= 10f && OnJump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.down * JumpForce / 55f, ForceMode.Impulse);
        }

        if (rb.position.x - DashingBeginning >= 30f && isDashing)
        {
            rb.linearVelocity = new Vector3(0, 0, rb.linearVelocity.z);
            //rb.AddForce(Vector3.down * JumpForce / 55f, ForceMode.Impulse);
        }
    }


    void SistemaPostura() // Sistema de aumento de velocidade
    {
        if (!DamageInvulnerability)
        {
            Speed += MultiplicadorVelocidade * Time.deltaTime / 5f;
        }
    }

    //função para espinhos detectarem colisão
    public void Hit(float damage)
    {
        DamageInvulnerability = true;
        //detecta colisão para parar de incrementar a velocidade

        Speed -= damage;
        //reduz a speed com base na vida

        MultiplicadorVelocidade /= 5f;
        //reduz a taxa de regeneração

        DamageInvulnerability = false;
        //volta a incrementar velocidade
    }


    void Mover() // Sistema de Corrida infinita
    {
        if (canMove)
        {
            rb.position += Vector3.right * Speed * Time.deltaTime;
        }
    }

    //Pulo do jogador -> INÍCIO

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Floor"))
        //lembrar que com colisões, precisamos acessar o gameObject deles para pegar coisas como tags
        {
            CanJump = true;
            OnJump = false;
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
        }

        //Parte da Lava
        if (collisionInfo.gameObject.CompareTag("LAVA"))
        {
            //player para
            canMove = false;

            //player começa a descer
            Invoke("DisableLayersCollision", 0.3f);

            GameController.controller.Invoke("GameOver", 0.8f);
            //invoca depois de alguns segundos a tela de morte
        }
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

    //Pulo do jogador -> FINAL

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

    //Troca de Lanes -> FINAL


    IEnumerator Dash()
    {
        isDashing = true;
        tr.emitting = true;
        DashingBeginning = rb.position.x;

        rb.linearVelocity = Vector3.zero;

        rb.AddForce(Vector3.right * dashingpower, ForceMode.Impulse);

        //rb.MovePosition(rb.position + Vector3.right * dashingpower);

        yield return new WaitForSeconds(dashingtime);

        isDashing = false;
        tr.emitting = false;
        canDash = false;

        yield return new WaitForSeconds(dashingCD);

        canDash = true;

    }

    void ChecagemDash() // Checa se pode dar dash, e roda se possivel
    {
        if (canDash == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(Dash());
        }
    }
}