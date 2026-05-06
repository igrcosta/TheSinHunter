using UnityEngine;
using UnityEngine.UIElements;

public class MoveBackGround : MonoBehaviour
{

    public float velocidade = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveScenario();
    }



    void MoveScenario()
    {
        if (Time.timeScale == 0)
            return;
        transform.Translate(velocidade,0,0 * Time.deltaTime);
    }
}
