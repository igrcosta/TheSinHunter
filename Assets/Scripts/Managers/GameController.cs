using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("CHEATS")]
    public bool Cheating = false;

    [Header("UI Infos")]
    public float Distance;
    public float Points;

    [Header("Mecanicas")]
    public Vector3 PlayerTargetPosition;

    [Header("Doors / Obstacles")]
    public LeverScript ActualLever;
    public ObstacleScript ActualObstacle;

    [Header("Referencias")]
    public static GameController controller;
    public UIController UIManager;
    public Player playerRef;

    private void Update()
    {
        DistanceCalculator();
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

    public void DistanceCalculator()
    {
        if (playerRef != null)
        {
            if (playerRef.Speed == 0f)
            {
                return;
            }
            Distance += (playerRef.Speed * Time.deltaTime) / 10;
        }
    }
    public void AddPoints(float add)
    {
        Points += add;
    }

    public void GameOver()
    {
        SceneManager.LoadScene(2);
        Distance = 0;
        Points = 0;
    }

    private void CheatMode()
    {
        if (Cheating)
        {

        }
    }


}
