using Unity.Multiplayer.PlayMode;
using UnityEngine;
using static CameraFollowScript;

public class CameraTrigger : MonoBehaviour
{

    private CameraFollowScript camera;
    private void Start()
    {
        camera = Camera.main.GetComponent<CameraFollowScript>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && this.CompareTag("Top"))
        {
            camera.currentLayer = CameraLayer.Top;

        }
        if (other.CompareTag("Player") && this.CompareTag("Bottom"))
        {
            camera.currentLayer = CameraLayer.Bottom;

        }
        if (other.CompareTag("Player") && this.CompareTag("Middle"))
        {
            camera.currentLayer = CameraLayer.Middle;

        }
        if (other.CompareTag("Player") && this.CompareTag("Dungeon"))
        {
            camera.currentLayer = CameraLayer.Dungeon;
        }

        if (other.CompareTag("Player") && this.CompareTag("Watchtower"))
        {
            camera.currentLayer = CameraLayer.Watchtower;

        
        }

    }
}
