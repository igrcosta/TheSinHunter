using UnityEngine;

public class ChunkScript : MonoBehaviour
{
    //esse script só vai servir para as chunks sumirem depois de um tempo

    [SerializeField] float DespawningTime = 300f;

    void FixedUpdate()
    {
        DespawningTime--;
        if(DespawningTime == 0)
        {
            Destroy(gameObject);
        }
    }
}
