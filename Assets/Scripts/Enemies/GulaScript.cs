using NUnit.Framework;
using UnityEngine;

public class GulaScript : MonoBehaviour
{
    private Player pRef;
    [SerializeField] float JumpHeight = 2f;
    [SerializeField] float JumpTime = 2f;

    [SerializeField] GameObject attackTrigger;
    [SerializeField] GameObject GulaEnemy;

    private bool isAttacking = false;
    private bool HasAttacked = false;
    private float startHeight;

    //private float WalkSpeed = 6f;
    [SerializeField] float Damage = 20f;
    void Start()
    {
        pRef = GameController.controller.playerRef;
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
            //Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Enemy") && other.CompareTag("Player") && !pRef.isDashing)
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

    public void CallAttack()
    {
        isAttacking = true;
    }
}
