using UnityEngine;

public class ScriptKey : MonoBehaviour
{
    InputHandler InputScript;
    SlowCollider SlowTime;

    private void OnColliderEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            Destroy(gameObject);

            


        }
    }
}
