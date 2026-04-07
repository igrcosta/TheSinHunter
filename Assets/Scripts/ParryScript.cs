using UnityEngine;

public class ParryScript : MonoBehaviour
{
    // private Vector3 ParryEffect = new Vector3 (0f, 9.81f*3, 0f);
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player scriptPlayer = other.GetComponent<Player>();
            //acesso player

            scriptPlayer.CanJump = true;
            //permito ele pular

            //aumento um POUCO sua velocidade

            Invoke("Destroying", 0.1f);
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
