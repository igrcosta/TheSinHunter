using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] float SpikeDamage = 10f;
    void OnTriggerEnter(Collider other)
    {
    //    if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing)
    //    {
    //        Destroy(gameObject);
    //    }

        if (other.CompareTag("Player"))
        {
            GameController.controller.playerRef.Hit(SpikeDamage);
        }
    }
}
