using UnityEngine;

public class CheckPoints : MonoBehaviour
{

    [SerializeField] int CheckPointIndex;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        GameController.controller.UnlockCheckpoint(CheckPointIndex);


        GameController.controller.playerRef.ParticleCheckpoint();

        //}


    }


}
