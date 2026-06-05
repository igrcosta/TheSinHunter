using UnityEngine;

public class ScriptKey : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.KeysCollected += 1;
            Destroy(gameObject);
        }
    }
}
