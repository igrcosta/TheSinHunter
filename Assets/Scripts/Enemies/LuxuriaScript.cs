using UnityEngine;

public class LuxuriaScript : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] float PointsGuiven = 250;

    [Header("Mechanics")]
    [SerializeField] float Damage = 15f;
    [SerializeField] float pushDistance = 8f;

    [Header("Referencias")]
    private Rigidbody rb;
    private GameObject shield;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        shield = transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing && shield != null)
        {
            //quebrar escudo
            Destroy(shield);
            GameController.controller.playerRef.EnableDash();

            //jogar luxúria para trás (knockback)
            rb.MovePosition(rb.position + Vector3.right * pushDistance);

            //dar mais um dash pro jogador
            GameController.controller.playerRef.EnableDash();

        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing)
        {
            GameController.controller.playerRef.FinishDash();
            GameController.controller.AddPoints(PointsGuiven);
            Destroy(gameObject);
            //VASCO //foda
        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing == false)
        {
            GameController.controller.playerRef.Hit(Damage);
            Debug.Log("LUXÚRIA DEU DANO");
        }
    }
}
