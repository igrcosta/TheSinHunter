using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] bool SpawnNecessity = false;


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
        if (other.CompareTag("Player") && gameObject.CompareTag("Castle"))
        {
            ChunkGeneration.ChunkGenerator.SpawnCastle = true;
            
        }
    }
}
