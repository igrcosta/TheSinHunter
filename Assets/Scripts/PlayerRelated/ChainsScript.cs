using UnityEngine;

public class ChainsScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Detectei inimigos!");
        }
    }
}
