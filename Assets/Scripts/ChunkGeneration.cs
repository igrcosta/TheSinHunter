using UnityEngine;

public class ChunkGeneration : MonoBehaviour
{
    [Header("Chunks Prefabs")]
    [SerializeField] private GameObject[] PrefabsToSpawn;

    public static ChunkGeneration ChunkGenerator;

    void Awake()
    {
        ChunkGenerator = this;
    }

    public ChunkSpawner DecisionPoint;
    private float ActualDistance = 0;

    //cada chunk terá o tamanho de 70 no X
    //spawnar chunks de 70 em 70 no X
    //lembrar que a ideia é trabalhar com grupos de chunks, vamos começar com chunks individuais e aí vamos melhorar para grupos de chunks

    public void ChunkSpawning(bool needToSpawn)
    {
        //marcar onde a chunk será spawnada
        //selecionar item aleatório do array de chunks
        //spawnar esse item aleatório nesse local

        ActualDistance++;
        float XcoordinatesToSpawn = ActualDistance * 70;

        if (PrefabsToSpawn != null && PrefabsToSpawn.Length > 0 && needToSpawn)
        {
            int randomIndex = Random.Range(0, PrefabsToSpawn.Length);
            GameObject ChunkSelected = PrefabsToSpawn[randomIndex];
            Instantiate(ChunkSelected, new Vector3(XcoordinatesToSpawn, 0, 0), transform.rotation);
        }
        else
        {
            Debug.LogWarning("array vazio ou nulo");
        }
    }
}