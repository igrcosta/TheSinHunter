using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    [SerializeField] GameObject Options;
    private Transform DashButton;

    void Start()
    {
        DashButton = gameObject.transform.GetChild(2);
    }

    void Update()
    {

    }

    public void BotaoJogar()
    {

        SceneManager.LoadScene(1);

    }

    public void BotaoSair()
    {
        Application.Quit();
    }

    public void BotaoConfigON()
    {
        Options.SetActive(true);
        Time.timeScale = 0;
    }

    public void BotaoConfigOFF()
    {
        Options.SetActive(false);
        Time.timeScale = 1;
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Dash()
    {
        GameController.controller.playerRef.ExecutarDashMobile();
    }
}
