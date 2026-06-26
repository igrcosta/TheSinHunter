using UnityEngine;

public class GulaScript : MonoBehaviour
{
    Player player;
    GulaState state;
    private Animator animator;

    [Header("Death")]
    [SerializeField] float PointsGuiven = 150;

    [Header("Jump System")]
    [SerializeField] float JumpHeight = 2f;
    [SerializeField] float speedUp = 2f;
    [SerializeField] float speedDown = 2f;

        
    [SerializeField] GameObject startPos;
    [SerializeField] GameObject topPos;


    [Header("Attack System")]
    [SerializeField] float Damage = 20f;

    [Header("Referencias")] //Referencias e Variaveis privadas
    [SerializeField] GameObject attackTrigger;
    [SerializeField] GameObject GulaEnemy;
    private bool isAttacking = false;
    private bool goingUp = false;

    enum GulaState
    {
        Idle,
        GoingUp,
        GoingDown,
        Dead
    }

  


    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        player = GameController.controller.playerRef;
        state = GulaState.Idle;
    }
    void Update()
    {

        switch (state)
        {
            case GulaState.GoingUp:
                MoveUp();
                break;

            case GulaState.GoingDown:
                MoveDown();
                break;
        }


        //if (!isAttacking) return;
        //Debug.Log(transform.position);
        //if (goingUp)
        //{
        //    //transform.position += Vector3.up * JumpHeight * Time.deltaTime * SpeedJump;
        //    //transform.position += Vector3.left * JumpHeight * Time.deltaTime * SpeedJump;

        //    transform.position = Vector3.MoveTowards(transform.position, topPos.transform.position, SpeedJump * Time.deltaTime);

        //    if (Vector3.Distance(transform.position, topPos.transform.position) < 0.01f)
        //    {
        //        goingUp = false;
        //    }

        //}
        //else
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, startPos.transform.position, fallSpeed * Time.deltaTime);

        //    if (Vector3.Distance(transform.position, startPos.transform.position) < 0.01f)
        //    {
        //        isAttacking = false;
        //    }
        //}

        ////if (transform.position.y > startHeight + 20f)
        ////{
        ////    isAttacking = false;
        ////    HasAttacked = true;
        ////    JumpHeight *= -1f;
        ////}
        ////if (transform.position.y <= startHeight - 0.2f)
        ////{
        ////    JumpHeight = 0f;
        ////}
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Enemy") && other.CompareTag("Player") && GameController.controller.playerRef.ExplosionState)
        {
            GameController.controller.playerRef.ContinuousRageExplosion();
            GameController.controller.AddPoints(PointsGuiven);
            Die();
        }
        else if (gameObject.CompareTag("Enemy") && other.CompareTag("Player") && !GameController.controller.playerRef.isDashing)
        {
            //se o player bateu na gula sem dar dash, player recebe dano

            GameController.controller.playerRef.Hit(Damage);
        }
        else if (gameObject.CompareTag("Enemy") && other.CompareTag("Player") && GameController.controller.playerRef.isDashing)
        {
            //se a gula bateu no player com ele dando dash, matar gula e seu trigger
            GameController.controller.playerRef.FinishDash();
            GameController.controller.AddPoints(PointsGuiven);
            Die();
        }
    }

    public void CallAttack()
    {
        if (isAttacking) return;
        isAttacking = true;
        goingUp = true;

        if (state != GulaState.Idle) return;

        state = GulaState.GoingUp;
    }

    void MoveUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, topPos.transform.position, speedUp * Time.deltaTime);

        animator.Play("Gula RIG|ENTREGAAtaque");

        if (Vector3.Distance(transform.position, topPos.transform.position) < 0.05f)
        {
            state = GulaState.GoingDown;

        }
    }

    void MoveDown()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            startPos.transform.position,
            speedDown * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, startPos.transform.position) < 0.05f)
        {
            state = GulaState.Idle;
        }
    }

    void Die()
    {
        state = GulaState.Dead;

        if (attackTrigger != null)
            (attackTrigger).SetActive(false);

        gameObject.SetActive(false);
    }

}
