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


        //if (other.CompareTag("Player"))
        //{
            GameController.controller.CurrentCheckpoint = CheckPointIndex;

            GameController.controller.UnlockedCheckpoints[CheckPointIndex] = true;

        GameController.controller.playerRef.ParticleCheckpoint();

            PlayerPrefs.SetInt("CurrentCheckpoint", CheckPointIndex);
            PlayerPrefs.SetInt("CheckpointUnlocked" + CheckPointIndex, 1);
            PlayerPrefs.Save();
        //}


    }


}
