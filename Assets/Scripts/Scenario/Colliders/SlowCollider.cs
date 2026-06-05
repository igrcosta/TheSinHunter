using UnityEngine;

public class SlowCollider : MonoBehaviour
{
    bool SlowingDown;
    [SerializeField] float SlowCap;
    [SerializeField] float HowMuchSlow;

    Player Pr;


    private void Start()
    {
        Pr = GameController.controller.playerRef;
    }

    void Update()
    {
        if (!SlowingDown) return;
        SlowPlayer();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InputHandler>().ColliderCinematic = true; //Avisa para o input handler que é uma cinematic
            SlowingDown = true;
        }
    }

    void SlowPlayer()
    {
        if (SlowingDown)
        {
            Pr.canAccelerate = false;
            Pr.canSlowDown = true;
            SlowingDown = true;
            if (Pr.Speed <= 0 || Pr.Speed <= SlowCap) return;
            Pr.Speed -= HowMuchSlow * Time.deltaTime;
        }
    }
    public void AcelleratePlayer ()
    {
        Pr.canAccelerate = true;
        SlowingDown = false;
        Pr.SpeedSystem();
    }
}
