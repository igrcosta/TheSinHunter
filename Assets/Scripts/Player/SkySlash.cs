using UnityEngine;

public class SkySlash : MonoBehaviour
{
    private Player Pref;
    [SerializeField] GameObject TargetPosition;

    [SerializeField] float SlashDamage = 5f;
    [SerializeField] float SlashSpeed = 40f;
    private float LifeTime = 2f;

    private void Start()
    {
        Pref = GameController.controller.playerRef;
        SlashSpeed = Pref.Speed * 4;
    }
    void Update()
    {
        if (Pref.SlashActive) 
            Move();
        LifeTime -= 1 * Time.deltaTime;
        if (LifeTime <= 0) Destroy(gameObject);
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition.transform.position, SlashSpeed * Time.deltaTime);
    }
}
