using Unity.VisualScripting;
using UnityEngine;

public class LeverScript : MonoBehaviour
{

    GameObject LeverModel;
    float angulo = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController.controller.ActualLever = this;
        LeverModel = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        LeverModel.transform.rotation = Quaternion.Euler(angulo, 0, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing || GameController.controller.playerRef.ExplosionState)
        {
            GameController.controller.ActualObstacle.UnlockGate();
            GameController.controller.playerRef.FinishDash();
            Destroy(gameObject);
            angulo = 130;

        }
    }
}
