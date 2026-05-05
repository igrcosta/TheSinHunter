using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] bool SpawnNecessity = false;
    bool Castle = false;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("DecisionPoints"))
        {
            ChunkGeneration.ChunkGenerator.ChunkSpawning(SpawnNecessity);
        }
        if (other.CompareTag("Player") && gameObject.CompareTag("DecisionPoints") && SpawnNecessity)
        {
            ChunkGeneration.ChunkGenerator.DecisionPoint = this;
        }
        if (other.CompareTag("Player") && gameObject.CompareTag("Castle") && !Castle)
        {
            Castle = true;
            if (ChunkGeneration.ChunkGenerator.SpawnCastle != true)
            {
                ChunkGeneration.ChunkGenerator.SpawnCastle = true;
            }
        }
        if (other.CompareTag("Player") && gameObject.CompareTag("Castle") && Castle)
        {
            Castle = false;
            if (ChunkGeneration.ChunkGenerator.SpawnCastle = true)
            {
                ChunkGeneration.ChunkGenerator.SpawnCastle = false;
            }
        }
        if (other.CompareTag("Player") && gameObject.CompareTag("Victory"))
        {
            GameController.controller.Victory();
        }


    }
}
