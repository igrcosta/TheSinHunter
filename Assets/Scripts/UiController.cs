using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject Options;
    public GameObject ChainsAim;

    #region singleton

    //coisas importantes para singleton -> INÍCIO
    public static UIController UIcontroller;

    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if (UIcontroller == null)
        {
            UIcontroller = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    //coisas importantes para singleton -> FIM

    #endregion singleton

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

    //lógica para mira das correntes conversar com outros scripts INÍCIO

    public void EnableAim()
    {
        ChainsAim.SetActive(true);
    }

    public void DisableAim()
    {
        ChainsAim.SetActive(false);
    }

    public void SetAimPosition(Vector3 PositionToSet)
    {
        ChainsAim.transform.position = PositionToSet;
    }

    //lógica para mira das correntes conversar com outros scripts FIM
}
