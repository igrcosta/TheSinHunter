using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("CHEATS")]
    public bool Cheating = false;

    [Header("UI Infos")]
    public float Distance;
    public float Points;
    public bool[] TutorialID = new bool[10];

    public float Master;
    public float SFX;
    public float Music;

    [Header("Mecanicas")]
    public Vector3 PlayerTargetPosition;

    [Header("Doors / Obstacles")]
    public LeverScript ActualLever;
    public ObstacleScript ActualObstacle;
    public int KeysCollected;

    [Header("CheckPoints")]
    public bool[] UnlockedCheckpoints = new bool[20];
    public int CurrentCheckpoint = 0;
    public Transform[] CheckPointPositions;
    [SerializeField] GameObject[] Enemies;
    public bool ResetEnemies;


    [Header("Player")]
    public bool ChainsUnlocked;
    public bool DoubleJumpUnlocked;
    public bool SuperDashUnlocked;
    public bool SkySlashUnlocked;

    public int ChainsUnlock = 0;
    public int SuperDashUnlock = 0;
    public int DoubleJumpUnlock = 0;
    public int SkySlashUnlock = 0;


    [Header("Referencias")]
    public static GameController controller;
    public UIController UIManager;
    public Player playerRef;


    private void Start()
    {
        ChainsUnlock = PlayerPrefs.GetInt("ChainsUnlock");
        SuperDashUnlock = PlayerPrefs.GetInt("SuperDashUnlock");
        DoubleJumpUnlock = PlayerPrefs.GetInt("DoubleJumpUnlock");
        SkySlashUnlock = PlayerPrefs.GetInt("SkySlashUnlock");

       

        for (int i = 0; i < TutorialID.Length; i++)
        {
            TutorialID[i] = PlayerPrefs.GetInt("Tutorial" + i, 0) == 1;
        }


        for (int i = 0; i < UnlockedCheckpoints.Length; i++)
        {
            UnlockedCheckpoints[i] = PlayerPrefs.GetInt("CheckpointUnlocked" + i, 0) == 1;
        }


        CurrentCheckpoint = PlayerPrefs.GetInt("CurrentCheckpoint", 0);

        //PlayerPrefs.DeleteAll();

        WeaponsColected();

        CurrentCheckpoint = 0;
    }
    private void Update()
    {
        DistanceCalculator();
    }



    #region Singleton

    private void Awake()
    {

        TutorialID = new bool[10];
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
            Distance += (playerRef.Speed * Time.deltaTime) / 6;
        }
    }
    public void AddPoints(float add)
    {
        Points += add;
    }

    public void PlayerDied()
    {
        UIManager.ShowDeathPanel();
    }

    public void RespawnPlayer()
    {

        playerRef.transform.position = CheckPointPositions[CurrentCheckpoint].position;

        playerRef.rb.linearVelocity = Vector3.zero;
        playerRef.Speed = 30f;

        ResetAll();

        playerRef.ResetPlayer();
    }

    public void ResetAll()
    {

        for (int i = 0; i < Enemies.Length; i++)
        {
            Enemies[i].SetActive(true);
        }

    }

    public void TravelToCheckpoint(int index)
    {
        if (!UnlockedCheckpoints[index])
            return;

        CurrentCheckpoint = index;

        PlayerPrefs.SetInt("CurrentCheckpoint", index);

        PlayerPrefs.Save();

        playerRef.transform.position =
            CheckPointPositions[index].position;

        

        

        UIManager.CloseCheckpointMenu();

        
    }

    public void GameOver()
    {
        if (Cheating)
            return;
        SceneManager.LoadScene(2);
        Distance = 0;
        Points = 0;
    }

    public void Victory()
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



    public void WeaponsColected()
    {
        if (ChainsUnlock == 1)
        {
            ChainsUnlocked = true;
            PlayerPrefs.SetInt("ChainsUnlock", 1);

        }

        if (DoubleJumpUnlock == 1)
        {
            DoubleJumpUnlocked = true;
            PlayerPrefs.SetInt("DoubleJumpUnlock", 1);

        }

        if (SuperDashUnlock == 1)
        {
            SuperDashUnlocked = true;
            PlayerPrefs.SetInt("SuperDashUnlock", 1);

        }

        if (SkySlashUnlock == 1)
        {
            SkySlashUnlocked = true;
            PlayerPrefs.SetInt("SkySlashUnlock", 1);

        }


        PlayerPrefs.Save();
    }


}
