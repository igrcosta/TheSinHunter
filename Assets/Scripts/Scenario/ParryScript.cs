using UnityEngine;

public class ParryScript : MonoBehaviour
{
    private Vector3 ParryEffect = new Vector3(0f, 9.81f, 0f);

    [SerializeField] float parryforce = 8f;

    bool Malware;
    [SerializeField] private GameObject deathFX;



    public void Death()
    {
        Instantiate(deathFX, this.gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
       
            Player scriptPlayer = other.GetComponent<Player>();
            //acesso player

            //indico que isParry é true

            if (scriptPlayer.isDashing)
            {
                scriptPlayer.isparrying = true;
                scriptPlayer.CanDoubleJump = true;
                Debug.Log("Player Encostou");

                scriptPlayer.rb.AddForce(ParryEffect * parryforce, ForceMode.Impulse);
                //scriptPlayer.IgnoreDashLogic();
                scriptPlayer.EnableDash();
                //permito ele pular

                //aumento um POUCO sua velocidade

                //GameController.controller.playerRef.FinishDash();

                Invoke("Destroying", 0.1f);
            }
            else if (scriptPlayer.isDashing && scriptPlayer.ChainsActive)
            {
                scriptPlayer.isparrying = true; 
                scriptPlayer.CanDoubleJump = true;


                scriptPlayer.EnableDash();
                //permito ele pular

                //aumento um POUCO sua velocidade

                GameController.controller.playerRef.FinishDash();

                Invoke("Destroying", 0.1f);
            }
            else if (scriptPlayer.ExplosionState)
            {
                scriptPlayer.ContinuousRageExplosion();

            }
        }
    }
    void Destroying()
    {
        Destroy(gameObject);
    }


    //LEMBRE-SE DISSO:
    //EM VEL MÁXIMA, colocar parrys com distancia de 37 em X entre eles
    //Em VEM MÍNIMA, colocar parrys com distancia e 9 em X entre eles
}
