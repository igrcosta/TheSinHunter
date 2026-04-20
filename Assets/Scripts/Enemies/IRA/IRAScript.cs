using UnityEngine;

public class IRAScript : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] float pointsGuiven = 100;

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
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing)
        {
            GameController.controller.playerRef.FinishDash();
            GameController.controller.AddPoints(pointsGuiven);
            Destroy(gameObject);
        }
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing == false)
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
