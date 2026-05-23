using UnityEngine;

public class Column : MonoBehaviour
{
    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing || GameController.controller.playerRef.ExplosionState)
        {
            GameController.controller.playerRef.FinishDash();
            GameController.controller.playerRef.EnableDash();
            Destroy(gameObject);
            

        }
    }
}
