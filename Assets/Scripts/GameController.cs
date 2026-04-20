using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("CHEATS")]
    public bool Cheating = false;

    public Vector3 PlayerTargetPosition;

    [Header("UI INFOS")]
    public float Distancia;
    public float Pontos;

    [Header("Referencias")]
    public static GameController controller;
    public UIController UIManager;
    public Player playerRef;

    private void Update()
    {
        if (playerRef.Speed == 0f)
            return;
        Distancia += (playerRef.Speed * Time.deltaTime) / 10;
    }
        
    #region Singleton

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
    #endregion Singleton

    public void GameOver()
    {
        SceneManager.LoadScene(2);
        //talvez seja melhor no futuro colocar um canvas pra ativar na cena do jogo mesmo
    }

    private void CheatMode()
    {
        if (Cheating)
        {

        }
    }
}
