using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] bool SpawnNecessity = false;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChunkGeneration.ChunkGenerator.ChunkSpawning(SpawnNecessity);
        }
        if (other.CompareTag("Player") && SpawnNecessity)
        {
            ChunkGeneration.ChunkGenerator.DecisionPoint = this;
        }
        if (other.CompareTag("Player") && gameObject.CompareTag("Castle"))
        {
            ChunkGeneration.ChunkGenerator.SpawnCastle = true;

        }
    }
}
