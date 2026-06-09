using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class CameraFollowScript : MonoBehaviour
{
    //Referencias
    private Player Pref;

    public CameraLayer currentLayer;
    float TargetY;
    float baseY;

    void Start()
    {
        if (GameController.controller.playerRef != null)
        {
            Pref = GameController.controller.playerRef;
        }
        else
        {
            Debug.Log("Player vazio meu filho");
        }
     
    }
    void Update()
    {
        if (Pref.Speed <= 20)
            return;

        XFollowing();

        switch (currentLayer)
        {
           

            case CameraLayer.Top:
                baseY = 40;
                break;

            case CameraLayer.Middle:
                baseY = 15;
                break;

            case CameraLayer.Bottom:
                baseY = -15;
                break;

            case CameraLayer.Dungeon:
            baseY = - 70;
                break;

            case CameraLayer.Watchtower:
                baseY = 100;
                break;
        }

        float localYOffset = Pref.transform.position.y - baseY;

        if (currentLayer == CameraLayer.Middle)
        localYOffset = Mathf.Clamp(localYOffset, 0f, 15f);

        if (currentLayer == CameraLayer.Top)
            localYOffset = Mathf.Clamp(localYOffset, -5f, 15f);

        if (currentLayer == CameraLayer.Watchtower)
            localYOffset = Mathf.Clamp(localYOffset, -20f, 30f);

        if (currentLayer == CameraLayer.Bottom)
            localYOffset = Mathf.Clamp(localYOffset, -10f, 5f);

        if (currentLayer == CameraLayer.Dungeon)
            localYOffset = Mathf.Clamp(localYOffset, -10f, 25f);


        TargetY = baseY + localYOffset;

        Vector3 targetPos = new Vector3(Pref.transform.position.x + 25f, TargetY,transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPos,0.1f);
    }

    public enum CameraLayer
    {
        Top,
        Middle,
        Bottom,
        Watchtower,
        Dungeon

    }
    void XFollowing()
    {
        transform.position = new Vector3(Pref.transform.position.x + 25f, transform.position.y, transform.position.z);
    }
}
