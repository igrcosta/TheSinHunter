using UnityEngine;

public class SkySlash : MonoBehaviour
{
    private Player Pref;
    [SerializeField] GameObject TargetPosition;

    [SerializeField] float SlashDamage = 5f;
    [SerializeField] float attackSpeed = 40f;
    private float LifeTime = 2f;

    private void Start()
    {
        Pref = GameController.controller.playerRef;
    }

    // Update is called once per frame
    void Update()
    {
        if (Pref.SlashActive) 
            Attack();
        LifeTime -= 1 * Time.deltaTime;
        if (LifeTime <= 0) Destroy(gameObject);
    }

    void Attack()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition.transform.position, attackSpeed * Time.deltaTime);
    }


}
