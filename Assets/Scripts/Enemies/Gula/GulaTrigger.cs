using UnityEngine;

public class GulaTrigger : MonoBehaviour
{
    [SerializeField] GulaScript GulaRef;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //se o trigger da gula, encostar com o player

            //chamar gula pra fazer seu ataque

            GulaRef.CallAttack();

        }
    }
}
