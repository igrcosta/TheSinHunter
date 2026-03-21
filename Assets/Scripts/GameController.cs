using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("CHEATS")]
    [SerializeField] bool Cheating = false;

    //coisas importantes para singleton -> INÍCIO
    public static GameController controller;

    private void Awake()
    {
        Singleton();
    } 

    private void Singleton()
    {
        if (controller == null)
        {
            controller = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    //coisas importantes para singleton -> FIM

    public void GameOver()
    {
        SceneManager.LoadScene(2);
        //talvez seja melhor no futuro colocar um canvas pra ativar na cena do jogo mesmo
    }



}
