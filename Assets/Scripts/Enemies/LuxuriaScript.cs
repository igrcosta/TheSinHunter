using System;
using UnityEngine;

public class LuxuriaScript : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject shield;
    //private float WalkSpeed = 6f;
    private Player pRef;
    [SerializeField] float Damage = 15f;
    [SerializeField] float pushDistance = 8f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shield = transform.GetChild(0).gameObject;

        pRef = GameController.controller.playerRef;
    }
    void Update()
    {
        //Movement();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && pRef.isDashing && shield != null)
        {
            //quebrar escudo
            Destroy(shield);

            //jogar luxúria para trás (knockback)
            rb.MovePosition(rb.position + Vector3.right * pushDistance);

            //dar mais um dash pro jogador
            pRef.EnableDash();

        }
        else if (other.CompareTag("Player") && pRef.isDashing)
        {
            Destroy(gameObject);
            //VASCO
        }
        else if (other.CompareTag("Player") && pRef.isDashing == false)
        {
            pRef.Hit(Damage);
            Debug.Log("LUXÚRIA DEU DANO");
        }
    }
}
