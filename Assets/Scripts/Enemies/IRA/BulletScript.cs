using System;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float bulletSpeed = 40f;
    private float LifeTime = 400f;
    [SerializeField] float BulletDamage = 5f;
    void Update()
    {
        Movement();

        LifeTime--;
        if (LifeTime <= 0) Destroy(gameObject);
    }

    private void Movement()
    {
        transform.position += Vector3.left * bulletSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing)
        {
           
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player") &&  GameController.controller.playerRef.ExplosionState)
        {
            //GameController.controller.playerRef.EnableDash();

            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            GameController.controller.playerRef.Hit(BulletDamage);
            Debug.Log("BALA DEU DANO");
            Destroy(gameObject);
        }
    }
}
