using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{


    [Header ("Sistema De Corrida")]
    [SerializeField] float speedcrescente = 1.5f;
    [SerializeField] int KnockBack = 1;
    [SerializeField] float Tempo_Voltaramover = 1.5f;

    [Header("Postura")]
    [SerializeField] int Postura = 1;
    [SerializeField] float Taxaderegeneracao = 1;

    [Header("Valores para pulo")]
    [SerializeField] float JumpForce = 20f;
    private Vector3 JumpVector;



    //Variaveis privadas
    float RegeneracaoPostura = 1;
    bool Colidiu = false;
    Rigidbody rb;
    public Animator mAnimator;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    void FixedUpdate()
    {
        Jumping();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            mAnimator.SetBool("Colidiu", true);
        }

        Mover();
       StartCoroutine("SistemaSpeed");
       QuebradePostura();
        RecuperacaoPostura();
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

        Vector3 V3Move = new Vector3(MoveX, 0, 0);

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

    void Jumping()
    {
        //script para o player poder pular

        JumpVector = new Vector3(0f,JumpForce,0f);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.AddForce(JumpVector, ForceMode.Impulse);
        }
    }
}