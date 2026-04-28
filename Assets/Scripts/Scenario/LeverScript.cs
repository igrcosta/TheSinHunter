using UnityEngine;

public class LeverScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController.controller.ActualLever = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing || GameController.controller.playerRef.ExplosionState)
        {
            GameController.controller.ActualObstacle.UnlockGate();
            GameController.controller.playerRef.FinishDash();
            Destroy(gameObject);
        }
    }
}
