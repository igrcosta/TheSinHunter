using UnityEngine;

public class IRAScript : MonoBehaviour
{
    private Rigidbody rb;
    private Transform ShootPoint;
    private bool CanShoot = false;
    [SerializeField] float Damage = 5f;

    [SerializeField] GameObject BulletPrefab;

    [SerializeField] float shootCooldown = 1.5f;
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
            Destroy(gameObject);
        }
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing == false)
        {
            GameController.controller.playerRef.Hit(Damage);
            Debug.Log("IRA DEU DANO");
        }
    }

    //lógica para tiro

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
}
