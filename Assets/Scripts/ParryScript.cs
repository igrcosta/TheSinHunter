using UnityEngine;

public class ParryScript : MonoBehaviour
{
    private Vector3 ParryEffect = new Vector3(0f, 9.81f, 0f);

    [SerializeField] float parryforce = 8f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player scriptPlayer = other.GetComponent<Player>();
            //acesso player

            scriptPlayer.ParryLogicEnable();
            //indico que isParry é true

            if (scriptPlayer.isDashing)
            {
                scriptPlayer.rb.AddForce(ParryEffect * parryforce, ForceMode.Impulse);
                //scriptPlayer.IgnoreDashLogic();
                scriptPlayer.EnableDash();
                //permito ele pular

                //aumento um POUCO sua velocidade

                Invoke("Destroying", 0.1f);
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
