using UnityEngine;

public class AnimationsPlayer : MonoBehaviour
{
    Player Pref;
    public Animator anim;

    void Start()
    {
        Pref = GameController.controller.playerRef;

    }

    void Update()
    {
        //anim.SetTrigger("Dash");

        anim.SetBool("Jump", Pref.Jumping);
        anim.SetBool("Falling", Pref.Falling);
        anim.SetBool("Attack", Pref.isDashing);
       


    }

    //public void Jump()
    //{

    //    anim.SetBool("JumpNow", true);


    //}

    public void Fall()
    {

        anim.SetBool("Fall", true);


    }

    public void Dash()
    {
        anim.SetTrigger("Dash");

    }
    public void Hit()
    {
        anim.SetTrigger("Hit");
        
    }
    public void DoubleJump()
    {
        Pref.Falling = false;
    }

    public void Death()
    {
        anim.SetTrigger("Death");
    }

    public void HitDeath()
    {
        anim.SetTrigger("HitDeath");
    }
    public void Drowned()
    {
        anim.SetTrigger("Drowned");
    }


    public void Chains()
    {
        anim.SetTrigger("Chains");

    }



    public void BackRun()
    {
       
        anim.SetBool("Jump", false);

    }


}
