using UnityEngine;

public class ChunkScript : MonoBehaviour
{
    //esse script só vai servir para as chunks sumirem depois de um tempo

    [SerializeField] float DespawningTime = 300f;
    [SerializeField] bool TESTING = false;

    [SerializeField] GameObject LanesCastle;



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

        if (ChunkGeneration.ChunkGenerator.SpawnCastle == true)
        {
            LanesCastle.SetActive(true);
        }

    }
}
