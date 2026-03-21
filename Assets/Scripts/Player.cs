using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using Unity.Android.Gradle.Manifest;
using System;

public class Player : MonoBehaviour
{


    [Header("Sistema De Corrida")]
    [SerializeField] float speedcrescente = 1.5f;
    [SerializeField] int KnockBack = 1;
    [SerializeField] float Tempo_Voltaramover = 1.5f;

    [Header("Postura")]
    [SerializeField] int Postura = 1;
    [SerializeField] float Taxaderegeneracao = 1;

    [Header("Infos para Pulo")]
    [SerializeField] float JumpForce = 10f;
    private Vector3 JumpVector;
    public bool CanJump = false;

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



    //Variaveis privadas
    float RegeneracaoPostura = 1;
    bool Colidiu = false;
    public Rigidbody rb;
    public Animator mAnimator;
    private Vector3 V3Move;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        else if (rb.linearVelocity.y <0)
        {
            currentGravityScale = fallingGravityScale;
        }
    }

    void FixedUpdate()
    {
        Mover();
        StartCoroutine("SistemaSpeed");
        QuebradePostura();
        RecuperacaoPostura();

        //lógica de física melhorada aqui, isso vai permitir uma queda irada pro player
        rb.AddForce(Physics.gravity * (gravityScale -1) * rb.mass);
        
        //(pelamor de Deus, rigidbody pra player é quase tentar ganhar uma triatlo sem saber nadar, tudo começa bem, mas no final...)

    }

    void Update()
    {
        Jumping();

        DescendingLanes();
    }

    IEnumerator SistemaSpeed() // Sistema de aumento de velocidade
    {
        if (speedcrescente < 50)
            speedcrescente += 0.3f;

        else if (speedcrescente >= 1)
        {
            yield return new WaitForSecondsRealtime(Tempo_Voltaramover);

            Colidiu = false;
            speedcrescente = 15;
            speedcrescente += 0.1f;

        }
    }



    void QuebradePostura()
    {
        if (Colidiu)
        {
            speedcrescente = 0;
        }

    }
    void Mover() // Sistema de Corrida infinita
    {
        float MoveX = 1;

        V3Move = new Vector3(MoveX, 0, 0);

        if (Colidiu == false)
        {
            rb.MovePosition(rb.position + V3Move * speedcrescente * Time.deltaTime);
        }
        else if (Colidiu)
        {
            rb.MovePosition(rb.position - V3Move * KnockBack * Time.deltaTime);
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
            QuebradePostura();
            Destroy(other);
        }
    }

    //Pulo do jogador -> INÍCIO

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Floor"))
        //lembrar que com colisões, precisamos acessar o gameObject deles para pegar coisas como tags
        {
            CanJump = true;
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
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.y);
            //zero a velocidade em y 

            rb.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
            //aplicamos a força em Y como impulso, de forma que mantenha a velocidade de X

            DisableLayersCollision();

            CanJump = false;
        }
    }

    //Pulo do jogador -> FINAL

    //Troca de Lanes -> INÍCIO
    void DescendingLanes()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && !IsOnALane && !CanJump)
        {
            rb.AddForce(Vector3.down * JumpForce / 1.5f, ForceMode.VelocityChange);
        }
        else
            if (Input.GetKeyDown(KeyCode.DownArrow) && IsOnALane)
            {
                DisableLayersCollision();
                rb.AddForce(Vector3.down * JumpForce / 1.7f, ForceMode.VelocityChange);
            }
        //primeiro ao apertar seta pra baixo



        //depois pra quando pular e esbarrar em algo da tag Lane
    }

    void EnableLayersCollision()
    {
        Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, false);
    }

    void DisableLayersCollision()
    {
        Physics.IgnoreLayerCollision(LanesLayer, PlayerLayer, true);
        Invoke("EnableLayersCollision", 0.3f);
    }

    //Troca de Lanes -> FINAL
}