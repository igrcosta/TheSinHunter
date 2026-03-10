using UnityEngine;

public class ParryScript : MonoBehaviour
{
    public Vector3 ParryEffect = new Vector3 (0f, 9.81f*3, 0f);
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player scriptPlayer = other.GetComponent<Player>();
            scriptPlayer.rb.AddForce(ParryEffect, ForceMode.Impulse);
            scriptPlayer.CanJump = true;
        }
    }
}
