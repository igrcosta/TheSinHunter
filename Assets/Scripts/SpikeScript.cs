using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] float SpikeDamage = 10f;
    private float LifeTime = 300f;

    void Update()
    {
        LifeTime--;
        if (LifeTime <= 0) Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.playerRef.Hit(SpikeDamage);
            Debug.Log("Encostei no player e mandei a mensagem");
        }
    }
}
