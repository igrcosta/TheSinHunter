using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using Unity.Android.Gradle.Manifest;
using System;

public class Player : MonoBehaviour
{


    [Header ("Sistema De Corrida")]
    [SerializeField] float speedcrescente = 1.5f;
    [SerializeField] int KnockBack = 1;
    [SerializeField] float Tempo_Voltaramover = 1.5f;

    [Header("Postura")]
    [SerializeField] int Postura = 1;
    [SerializeField] float Taxaderegeneracao = 1;

    [Header("Infos para Pulo")]
    [SerializeField] float JumpForce = 100f;
    private Vector3 JumpVector;
    private float gravity = -9.81f;
    private Vector3 GravityFactor;
    public bool CanJump = false;

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
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mAnimator.SetBool("Colidiu", true);
        }

        Mover();
       StartCoroutine("SistemaSpeed");
       QuebradePostura();
        RecuperacaoPostura();
    }

    void Update()
    {
        Jumping();

        GravityAction();

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
            mAnimator.SetTrigger("TrKnockback");
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
        if(collisionInfo.gameObject.CompareTag("Lane"))
        {
            CanJump = true;
            /* LaneCollider = collisionInfo.gameObject.GetComponent<BoxCollider>(); */
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
            JumpVector = new Vector3(0f,JumpForce, 0f);
            //definimos um vetor com a força que queremos que o jogador pule

            rb.AddForce(JumpVector, ForceMode.Impulse);
            //aplicamos a força em Y como impulso, de forma que mantenha a velocidade de X

            DisableLayersCollision();

            CanJump = false;
        }
    }

    void GravityAction()
    {
        GravityFactor = new Vector3(0f, gravity, 0f);
        rb.AddForce(GravityFactor, ForceMode.Acceleration);
    }

    //Pulo do jogador -> FINAL

    //Troca de Lanes -> INÍCIO
    void DescendingLanes()
    {
        if (Input.GetKey(KeyCode.DownArrow) && IsOnALane)
        {
            DisableLayersCollision();
            rb.AddForce(GravityFactor*2, ForceMode.Acceleration);
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
        Invoke("EnableLayersCollision", 0.5f);
    }

    


        //Troca de Lanes -> FINAL
}