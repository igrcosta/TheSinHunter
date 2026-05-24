using UnityEngine;

public class FakeParryScript : MonoBehaviour
{

    FakeParryState State;
    Player player;

    [Header("Distâncias")]
    [SerializeField] float wakeDistance = 35f;

    [Header("Ataque")]
    [SerializeField] float attackSpeed = 35f;
    [SerializeField] float attackDuration = 1f;
    [SerializeField] float Damage = 5f;
    [SerializeField] GameObject Movetarget;


    [Header("Visual")]
    [SerializeField] GameObject armsObject;

    private bool awakened = false;

    void Start()
    {
        player = GameController.controller.playerRef;

        if (armsObject != null)
        {
            armsObject.SetActive(false);
        }
    }

    enum FakeParryState
    {
        Idle,
        Awakening,
        Attacking
    }

    void Update()
    {
        switch (State)
        {
            case FakeParryState.Idle:
                IdleState();
                break;

            case FakeParryState.Awakening:
                break;

            case FakeParryState.Attacking:
                AttackState();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag("Player"))
        {
            GameController.controller.playerRef.Hit(Damage);
            Debug.Log("AVAREZA DEU DANO");
        }
    }

    void IdleState()
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= wakeDistance && !awakened)
            {
                awakened = true;

                State = FakeParryState.Awakening;

                WakeUp();
            }
        }

    void WakeUp()
    {
        Debug.Log("ACORDEI");

        if (armsObject != null)
        {
            armsObject.SetActive(true);
        }

        Invoke(nameof(BeginAttack), 0.4f);
    }

    void BeginAttack()
    {
        State = FakeParryState.Attacking;

        Invoke(nameof(StopAttack), attackDuration);
    }
    void AttackState()
    {
        transform.position = Vector3.MoveTowards(transform.position, Movetarget.transform.position, attackSpeed * Time.deltaTime);
    }

    void StopAttack()
    {
        Destroy(gameObject);
    }




}
