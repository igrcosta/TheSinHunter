using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    private Player player;
    private bool Unlocked = false;

    private float targetY;
    private Vector3 TargetPosition;

    private Vector3 initialScale;
    private Vector3 targetScale;

    void Start()
    {
        GameController.controller.ActualObstacle = this;

        player = GameController.controller.playerRef;

        targetY = transform.position.y + 40f;

        TargetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

        initialScale = transform.localScale;
        targetScale = new Vector3(0f, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Unlocked && gameObject.name == "VDOOR")
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.08f);

            Invoke("Destroying", 2f);
        }
        else if (Unlocked && gameObject.name == "HDOOR")
        {
            //diminuir escala do malandro em X
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.03f);

            Invoke("Destroying", 2f);
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

    void Destroying()
    {
        Destroy(gameObject);
    }
}
