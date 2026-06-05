using UnityEngine;

public class TimeCollider : MonoBehaviour
{
    public bool stoptime = false;
    public float HowFast, Target=2f;

    float InitialFixedDeltaTime;
    InputHandler InputScript;

    private void Start()
    {
        InitialFixedDeltaTime = Time.fixedDeltaTime;
    }
    private void Update()
    {
        TimeBack();
        if (!stoptime) return;
            TimeStopper();
    }
    void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            stoptime = true;
            InputScript = other.GetComponent<InputHandler>();
            InputScript.SlowCinematic = true;
            Debug.Log("Game paused");
        }

    }

    void TimeStopper()
    {
        if (stoptime)
        {

            Time.timeScale = Mathf.Lerp(Time.timeScale, Target, HowFast * Time.unscaledDeltaTime); // Define o TimeScale

            Time.fixedDeltaTime = InitialFixedDeltaTime * Time.timeScale; //Deixa a fisica fluida, e o time stop tambem

            if (Time.timeScale < 0.01) // Caso o valor pra retirar deixe o time scale menor que 0 define para um valor fixo bem baixo
            {
                Time.timeScale = 0.001f;
            }

            Debug.Log("Escala de tempo atual: " + Time.timeScale);


        }
        if (Time.timeScale <= 0.001f)
        {
                stoptime = false; // Desliga o Update
                Debug.Log("Tempo parado completamente!");
        }

    }

    public void TimeBack()
    {
        if (InputScript.SlowCinematic == false)

       { if (Time.timeScale > 0.05f) return;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = InitialFixedDeltaTime;
        stoptime = false;
        }

    }
}
