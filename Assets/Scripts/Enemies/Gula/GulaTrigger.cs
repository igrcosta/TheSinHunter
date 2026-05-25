using UnityEngine;

public class GulaTrigger : MonoBehaviour
{
    [SerializeField] GulaScript GulaRef;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ALGUEM ENTROU");
        if (other.CompareTag("Player"))
        {
            //se o trigger da gula, encostar com o player
            Debug.Log("Player ENTROU");

            //chamar gula pra fazer seu ataque

            GulaRef.CallAttack();

        }
    }
}
