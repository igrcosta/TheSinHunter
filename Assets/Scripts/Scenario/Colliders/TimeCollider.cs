using UnityEngine;

public class TimeCollider : MonoBehaviour
{
    public bool stoptime = false;
    public float HowFast, Target = 2f;
    [SerializeField] GameObject Drawing;
    public bool isActive = false;
    bool Inactive = false;
    [SerializeField] int TutorialIndex;

    float InitialFixedDeltaTime;
    InputHandler InputScript;

    private void Start()
    {
        Debug.Log("TutorialIndex: " + TutorialIndex);
        Debug.Log("Array Size: " + GameController.controller.TutorialID.Length);

        if (GameController.controller.TutorialID[TutorialIndex] == true)
        {
            Destroy(gameObject);
        }

        InitialFixedDeltaTime = Time.fixedDeltaTime;
    }
    private void Update()
    {
        TimeBack();
        ShowDrawing();
        if (!stoptime) return;
            TimeStopper();
    }
    void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            GameController.controller.TutorialID[TutorialIndex] = true;

            stoptime = true;
            InputScript = other.GetComponent<InputHandler>();
            InputScript.SlowCinematic = true;
            isActive = true;
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

            //Debug.Log("Escala de tempo atual: " + Time.timeScale);


        }
        if (Time.timeScale <= 0.001f)
        {
                stoptime = false; // Desliga o Update
                Debug.Log("Tempo parado completamente!");
        }

    }
    void ShowDrawing()
    {
        if (isActive)
        {
            if (Time.timeScale < 0.15)
            {
                Drawing.SetActive(true);
            }
            else
            {
                Drawing.SetActive(false);
            }
        }
    }

    public void TimeBack()
    {
        if (InputScript == null)
           return;
        Inactive = true;

        if (InputScript.SlowCinematic == false)

       {
            if (Time.timeScale > 0.05f) return;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = InitialFixedDeltaTime;
        stoptime = false;
            Destroy(this);
        }
    }

    
}
