using UnityEngine;

public class Column : MonoBehaviour
{
    public void Death()
    {
        //Instantiate(deathFX, this.gameObject.transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing || GameController.controller.playerRef.ExplosionState)
        {
            GameController.controller.playerRef.FinishDash();
            GameController.controller.playerRef.EnableDash();
            Death();


        }
    }
}
