using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] float SpikeDamage = 10f;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.playerRef.SpikeHit(SpikeDamage);
            Debug.Log("Encostei no player e mandei a mensagem");
        }
    }
}
