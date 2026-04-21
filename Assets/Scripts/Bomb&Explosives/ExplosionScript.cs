using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] float DespawnTime = 0.15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("Dissapearing", DespawnTime);
        //desaparecer dps de um tempo

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FlowState()
    {
        //quando explodir e ele sair voando, habilitar bool de flowstate
        //no flowstate, trigger em inimigos = explosão + boost
    }
    void Dissapearing()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 10f);
    }
}
