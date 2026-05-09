using UnityEngine;

public class ChunkScript : MonoBehaviour
{
    //esse script só vai servir para as chunks sumirem depois de um tempo

    [SerializeField] float DespawningTime = 300f;
    [SerializeField] bool TESTING = false;
    [SerializeField] bool DownWards = false;


    [SerializeField] GameObject LanesCastle;

    ChunkGeneration ChunkGenerator;

    void FixedUpdate()
    {
        if (!TESTING)
        {
            DespawningTime--;
            if (DespawningTime == 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //nada
        }
        if (LanesCastle != null && ChunkGeneration.ChunkGenerator != null)
        {
            LanesCastle.SetActive(ChunkGeneration.ChunkGenerator.SpawnCastle);
        }

    }
}
