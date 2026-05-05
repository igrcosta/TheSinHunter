using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] bool SpawnNecessity = false;
    [SerializeField] int DownWards = 0;
    bool Castle = false;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("DecisionPoints"))
        {
            if (DownWards == 0)
            {
                ChunkGeneration.ChunkGenerator.ChunkSpawning(SpawnNecessity, DownWards);
            }
            else if(DownWards == 1) 
            {
                ChunkGeneration.ChunkGenerator.ChunkSpawningDownWards(SpawnNecessity, DownWards);

            }


        }
        
        if (other.CompareTag("Player") && gameObject.CompareTag("DecisionPoints") && SpawnNecessity)
        {
            ChunkGeneration.ChunkGenerator.DecisionPoint = this;
        }
        if (other.CompareTag("Player") && gameObject.CompareTag("Castle"))
        {
            if (ChunkGeneration.ChunkGenerator.SpawnCastle)
            {
                Castle = false;
                ChunkGeneration.ChunkGenerator.SpawnCastle = false;

            }
            else
            {
                Castle = true;
                ChunkGeneration.ChunkGenerator.SpawnCastle = true;

            }
        }
        
        
        if (other.CompareTag("Player") && gameObject.CompareTag("Victory"))
        {
            GameController.controller.Victory();
        }


    }
}
