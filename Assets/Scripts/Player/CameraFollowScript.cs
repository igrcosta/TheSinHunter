using UnityEngine;


public class CameraFollowScript : MonoBehaviour
{
    //Referencias
    private Player Pref;
    private Vector3 EvenUpwards;
    private Vector3 Upwards;
    private Vector3 Middle;
    private Vector3 DownWards;

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
        EvenUpwards = new Vector3(Pref.transform.position.x + 22, 28f, transform.position.z);

        Upwards = new Vector3(Pref.transform.position.x + 22, 18f, transform.position.z);

        Middle = new Vector3(Pref.transform.position.x + 22f, 15f, transform.position.z);

        DownWards = new Vector3(Pref.transform.position.x + 22f, 12f, transform.position.z);
    }
    void Update()
    {
        YFollowing();
        XFollowing();
    }

    void YFollowing()
    {
        if (Pref.transform.position.y >= 24)
        {
            transform.position = Vector3.Lerp(transform.position, EvenUpwards, 0.10f);
        }
        if (Pref.transform.position.y > 16)
        {
            transform.position = Vector3.Lerp(transform.position, Upwards, 0.10f);

        }
        else if (Pref.transform.position.y < 16 && Pref.transform.position.y > 12)
        {
            transform.position = Vector3.Lerp(transform.position, Middle, 0.10f);
        }
        else if (Pref.transform.position.y < 10)
        {
            transform.position = Vector3.Lerp(transform.position, DownWards, 0.10f);
        }
    }

    //o player fica nas seguintes regiões
    // 23Y -> cam em 32
    // 13Y -> cam em 22
    // 3Y -> cam em 12

    void XFollowing()
    {
        transform.position = new Vector3(Pref.transform.position.x + 20f, transform.position.y, transform.position.z);
    }
}
