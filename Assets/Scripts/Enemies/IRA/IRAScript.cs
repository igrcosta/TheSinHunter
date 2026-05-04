using UnityEngine;

public class IRAScript : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] float PointsGuiven = 100;

    [SerializeField] bool ComboEnabled = false;

    [Header("Shoot System")]
    [SerializeField] float Damage = 5f;
    [SerializeField] float shootCooldown = 1.5f;
    private bool CanShoot = false;

    [Header("Referencias")]
    [SerializeField] GameObject BulletPrefab;
    private Rigidbody rb;
    private Transform ShootPoint;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ShootPoint = gameObject.transform.GetChild(0);
        EnableShoot();
    }
    void Update()
    {
        Shoot();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.ExplosionState == true)
        {
            Debug.Log("RECEBA");
            GameController.controller.AddPoints(PointsGuiven);
            GameController.controller.playerRef.ContinuousRageExplosion();
            GameController.controller.playerRef.EnableDash();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.DefaultActive && GameController.controller.playerRef.isDashing && ComboEnabled)
        {
            GameController.controller.playerRef.ComboDash();
            Debug.Log("COMBOO");
            GameController.controller.AddPoints(PointsGuiven);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.ChainsActive && GameController.controller.playerRef.isDashing && ComboEnabled)
        {
            Debug.Log("COMBOO CORRENTE");
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
        else if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing == false)
        {
            GameController.controller.playerRef.Hit(Damage);
            Debug.Log("IRA DEU DANO");
        }
    }

    #region ShootingSystem
    void Shoot()
    {
        if (CanShoot)
        {
            Instantiate(BulletPrefab, ShootPoint.position, transform.rotation);
            Debug.Log("TOMA");
        }
    }

    //esse script abaixo serve para intercalar os tiros, cuidado

    void EnableShoot()
    {
        CanShoot = true;
        Invoke("DisableShoot", 0.01f);
    }
    void DisableShoot()
    {
        CanShoot = false;
        Invoke("EnableShoot", shootCooldown);
    }

    #endregion ShootingSystem

}
