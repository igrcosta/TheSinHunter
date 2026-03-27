using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using Unity.Android.Gradle.Manifest;
using System;

public class Player : MonoBehaviour
{


    [Header("Sistema De Corrida")]
    [SerializeField] float Speed = 1.5f;
    [SerializeField] float SpeedPostColission = 15;
    [SerializeField] float LimiteVelocidade = 1.5f;

    [SerializeField] float MultiplicadorVelocidade = 0.1f;

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
    bool Colidiu = false;
    public Rigidbody rb;
    public Animator mAnimator;
    private Vector3 V3Move;

    //variavel para poder parar o movimento do player quando quiser
    private bool canMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

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

        //lógica de física melhorada aqui, isso vai permitir uma queda irada pro player
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);

        //(pelamor de Deus, rigidbody pra player é quase tentar ganhar uma triatlo sem saber nadar, tudo começa bem, mas no final...)

    }

    void Update()
    {
        Mover();
        SistemaSpeed();
        RecuperacaoPostura();
        Jumping();
        DescendingLanes();

        if (rb.position.y - JumpingBeginning >= 10f && OnJump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.down * JumpForce / 55f, ForceMode.Impulse);
        }
    }


    void SistemaSpeed() // Sistema de aumento de velocidade
    {
        if (Speed < LimiteVelocidade)
        {
            Speed += MultiplicadorVelocidade * Time.deltaTime;
        }

        else if (Colidiu)
        {
            Speed = SpeedPostColission;
            Speed += MultiplicadorVelocidade * Time.deltaTime;
            Colidiu = false;
        }
    }


    void Mover() // Sistema de Corrida infinita
    {
        if (canMove)
        {
            rb.position += Vector3.right * Speed * Time.deltaTime;
        }
    }


    void RecuperacaoPostura()
    {
        if (Postura <= 1)
        {
            RegeneracaoPostura += Taxaderegeneracao;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        string Tagcolidida = other.tag;

        if (Tagcolidida == "Enemy") // parar o movimento ao colidir
        {
            Colidiu = true;
            Destroy(other.gameObject);
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

}