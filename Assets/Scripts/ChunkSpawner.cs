using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] bool SpawnNecessity = false;
    void Start()
    {
        ChunkGeneration.ChunkGenerator.DecisionPoint = this;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChunkGeneration.ChunkGenerator.ChunkSpawning(SpawnNecessity);
        }
    }
}
