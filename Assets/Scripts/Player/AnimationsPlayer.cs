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

       


    }

    public void Jump()
    {

        anim.SetBool("Jump", true);


    }

    public void Dash()
    {
        anim.SetTrigger("Dash");

    }
   
    

    public void BackRun()
    {
       
        anim.SetBool("Jump", false);

    }


}
