using Unity.Mathematics;
using UnityEngine;

public class IRAScript : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] float PointsGuiven = 100;

    [SerializeField] bool ComboEnabled = false;
    [SerializeField] bool Far = false;

    float seconds = 0;

    [Header("Shoot System")]
    [SerializeField] float Damage = 5f;
    [SerializeField] float shootCooldown = 1.5f;
    private bool CanShoot = false;

    [Header("Referencias")]
    [SerializeField] GameObject BulletPrefab;
    private Rigidbody rb;
    private Transform ShootPoint;
    [SerializeField]private GameObject deathFX; 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ShootPoint = gameObject.transform.GetChild(0);
        EnableShoot();
    }
    void Update()
    {
        Shoot();

        seconds += 1 * Time.deltaTime;
        
        if(seconds > 15)
        {
            Far = true;
        }
    }

    public void Death()
    {
        GameController.controller.AddPoints(PointsGuiven);
        Instantiate(deathFX, this.gameObject.transform.position, Quaternion.identity);
        gameObject.SetActive(false);

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.ExplosionState == true)
        {
            Debug.Log("RECEBA");
            GameController.controller.playerRef.ContinuousRageExplosion();
            GameController.controller.playerRef.EnableDash();
            Death();
        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.DefaultActive && GameController.controller.playerRef.isDashing && ComboEnabled)
        {
            GameController.controller.playerRef.ComboDash();
            GameController.controller.playerRef.EnableDash();
            Debug.Log("COMBOO");
            Death();
        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.ChainsActive && GameController.controller.playerRef.isDashing && ComboEnabled)
        {
            GameController.controller.playerRef.EnableDash();
            Debug.Log("COMBOO CORRENTE");
            Death();
        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing && !ComboEnabled)
        {
            GameController.controller.playerRef.FinishDash();
            Debug.Log("FUI COM GOD");
            Death();

        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing == false)
        {
            GameController.controller.playerRef.Hit(Damage);
            Debug.Log("IRA DEU DANO");
        }
        else if (other.CompareTag("SkySlash"))
        {
            Death();
        }
    }

    #region ShootingSystem
    void Shoot()
    {
        if (CanShoot)
        {
            if (!Far)
            {
                Instantiate(BulletPrefab, ShootPoint.position, transform.rotation);
                Debug.Log("TOMA");
            }
            else if (Far)
            {
                Instantiate(BulletPrefab, ShootPoint.position, transform.rotation);
            }
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
