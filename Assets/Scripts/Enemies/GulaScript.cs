using UnityEngine;

public class GulaScript : MonoBehaviour
{
    public Rigidbody rb;
    private Player pRef;
    [SerializeField] float JumpSpeed = 2f;

    [SerializeField] GameObject attackTrigger;
    [SerializeField] GulaScript GulaEnemy;

    //private float WalkSpeed = 6f;
    [SerializeField] float Damage = 20f;
    void Start()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            rb = GetComponent<Rigidbody>();
        }

        pRef = GameController.controller.playerRef;
    }
    void Update()
    {
        //Movement();
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("GulaTrigger") && other.CompareTag("Player"))
        {
            //se o trigger da gula, encostar com o player

            //chamar gula pra fazer seu ataque

            Destroy(attackTrigger);
            //destruir trigger para evitar bugs

            Attack();

        }
        else if (gameObject.CompareTag("Enemy") && other.CompareTag("Player") && !pRef.isDashing)
        {
            //se o player bateu na gula sem dar dash...

            //player recebe dano

            pRef.Hit(Damage);
            Debug.Log("GULA DEU DANO");
        }
        else if (gameObject.CompareTag("Enemy") && other.CompareTag("Player") && pRef.isDashing)
        {
            //se a gula bateu no player com ele dando dash...

            //matar gula e seu trigger
            Destroy(attackTrigger);
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        //gula vai pular para a lane superior e cair logo depois devolta para sua lane de origem


        GulaEnemy.rb.position = Vector3.Lerp(rb.position, Vector3.up * JumpSpeed, 0.10f);
        Debug.Log("eu vou pular!");
        Invoke("EndAttack", 2f);

        //exemplo:
        //transform.position = Vector3.Lerp(transform.position, Upwards, 0.10f);
    }
    void EndAttack()
    {
        GulaEnemy.rb.position = Vector3.Lerp(rb.position, Vector3.down * JumpSpeed, 0.10f);
    }
}
