using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    private Player player;
    private bool Unlocked = false;

    private float targetY;
    private Vector3 TargetPosition;

    void Start()
    {
        GameController.controller.ActualObstacle = this;

        player = GameController.controller.playerRef;

        targetY = transform.position.y + 31f;

        TargetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Unlocked)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.08f);

            /* if (targetY - transform.position.y >= 2f)
            {
                Destroy(gameObject);
            } */
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //verificar se a alavanca foi apertada ou não
            if (Unlocked)
            {
                //nada
            }
            else if (!Unlocked)
            {
                player.LavaKill();
            }
        }
    }

    public void UnlockGate()
    {
        //subir 29 em Y
        Unlocked = true;
    }
}
