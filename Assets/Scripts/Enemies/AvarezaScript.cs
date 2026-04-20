using UnityEngine;

public class AvarezaScript : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] float PointsGuiven = 100;

    [Header("Mechanics")]
    [SerializeField] float Damage = 20f;

    [Header("Referencias")]
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing)
        {
            GameController.controller.playerRef.FinishDash();
            Debug.Log("FUI COM GOD");
            GameController.controller.AddPoints(PointsGuiven);
            Destroy(gameObject);

        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing == false)
        {
            GameController.controller.playerRef.Hit(Damage);
            Debug.Log("AVAREZA DEU DANO");
        }
    }
}
