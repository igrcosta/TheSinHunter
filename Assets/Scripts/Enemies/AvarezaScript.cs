using UnityEngine;

public class AvarezaScript : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] float PointsGuiven = 100;


    [Header("Mechanics")]
    [SerializeField] float Damage = 20f;
    [SerializeField] bool ComboEnabled = false;

    [Header("Referencias")]
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing && ComboEnabled)
        {
            GameController.controller.playerRef.ComboDash();
            Debug.Log("COMBOO");
            GameController.controller.AddPoints(PointsGuiven);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing && !ComboEnabled)
        {
            GameController.controller.playerRef.FinishDash();
            Debug.Log("FUI COM GOD");
            GameController.controller.AddPoints(PointsGuiven);
            Destroy(gameObject);

        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.ExplosionState)
        {
            GameController.controller.playerRef.ContinuousRageExplosion();
            GameController.controller.playerRef.EnableDash();
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
