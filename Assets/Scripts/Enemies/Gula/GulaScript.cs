using UnityEngine;

public class GulaScript : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] float PointsGuiven = 150;

    [Header("Jump System")]
    [SerializeField] float JumpHeight = 2f;

    [Header("Attack System")]
    [SerializeField] float Damage = 20f;

    [Header("Referencias")] //Referencias e Variaveis privadas
    [SerializeField] GameObject attackTrigger;
    [SerializeField] GameObject GulaEnemy;
    private bool isAttacking = false;
    private bool HasAttacked = false;
    private float startHeight;


    void Start()
    {
        startHeight = transform.position.y;
    }
    void Update()
    {
        if (!isAttacking && !HasAttacked) return;

        transform.position += Vector3.up * JumpHeight * Time.deltaTime;

        if (transform.position.y > startHeight + 20f)
        {
            isAttacking = false;
            HasAttacked = true;
            JumpHeight *= -1f;
        }
        if (transform.position.y <= startHeight - 0.2f)
        {
            JumpHeight = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Enemy") && other.CompareTag("Player") && GameController.controller.playerRef.ExplosionState)
        {
            Debug.Log("EXPLODIU");
            GameController.controller.playerRef.ContinuousRageExplosion();
            GameController.controller.AddPoints(PointsGuiven);
            Destroy(gameObject);
        }
        else if (gameObject.CompareTag("Enemy") && other.CompareTag("Player") && !GameController.controller.playerRef.isDashing)
        {
            //se o player bateu na gula sem dar dash, player recebe dano

            GameController.controller.playerRef.Hit(Damage);
            Debug.Log("GULA DEU DANO");
        }
        else if (gameObject.CompareTag("Enemy") && other.CompareTag("Player") && GameController.controller.playerRef.isDashing)
        {
            //se a gula bateu no player com ele dando dash, matar gula e seu trigger
            GameController.controller.playerRef.FinishDash();
            GameController.controller.AddPoints(PointsGuiven);
            Destroy(attackTrigger);
            Destroy(gameObject);
        }
    }

    public void CallAttack()
    {
        isAttacking = true;
    }
}
