using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    [SerializeField] GameObject Options;

    void Start()
    {
        
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
}
