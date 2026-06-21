using UnityEngine;

public class AvarezaScript : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] float PointsGuiven = 100;


    [Header("Mechanics")]
    [SerializeField] float Damage = 5f;
    [SerializeField] bool ComboEnabled = false;
    [SerializeField] bool ChainsEnabled = false;

    [Header("FeedBacks")]
    [SerializeField] private GameObject deathFX;

    [Header("Referencias")]
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Death()
    {
        GameController.controller.AddPoints(PointsGuiven);
        Instantiate(deathFX, this.gameObject.transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.DefaultActive && GameController.controller.playerRef.isDashing && ChainsEnabled)
        {
            GameController.controller.playerRef.ComboDash();
            Debug.Log("COMBOO");
            GameController.controller.AddPoints(PointsGuiven);
            Death();
        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.ChainsActive && GameController.controller.playerRef.isDashing && ChainsEnabled)
        {
            GameController.controller.playerRef.ComboDash();
            Debug.Log("COMBOO");
            GameController.controller.AddPoints(PointsGuiven);
            Death();
        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing && !ComboEnabled)
        {
            //GameController.controller.playerRef.FinishDash();
            GameController.controller.playerRef.FinishDash();
            Debug.Log("FUI COM GOD");
            GameController.controller.AddPoints(PointsGuiven);
            Death();

        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.ExplosionState)
        {
            GameController.controller.playerRef.ContinuousRageExplosion();
            GameController.controller.playerRef.EnableDash();
            GameController.controller.AddPoints(PointsGuiven);
            Death();    

        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing == false)
        {
            GameController.controller.playerRef.Hit(Damage);
            Debug.Log("AVAREZA DEU DANO");
        }
    }
}
