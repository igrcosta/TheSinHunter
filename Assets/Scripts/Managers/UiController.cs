using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] GameObject OptionsPanel;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject AudioPanel;
    [SerializeField] GameObject[] CreditsPanel;
    [SerializeField] GameObject DeathPanel;
    [SerializeField] GameObject VictoryPanel;
    [SerializeField] GameObject CheckpointPanel;
    [SerializeField] GameObject [] TutorialPanel;

    [SerializeField] GameObject[] Haeds;
    [SerializeField] GameObject[] Maps;


    

    int currentCredit = 0;
    int currentTutorial = 0;
    public GameObject ChainsAim;
    public TMPro.TextMeshProUGUI DistanceText;
    public TMPro.TextMeshProUGUI PointsText;
    public bool isPause = false;

    [Header("Som")]
    public AudioSource audioSource;

    void Start()
    {
        GameController.controller.UIManager = this;
        MapsUnlocked();
    }


    private void Update()
    {
        ShowTextOnUI();
        
    }
    public void ShowTextOnUI()
    {
        if (DistanceText != null)
            DistanceText.text = GameController.controller.Distance.ToString("F0") + " M";
        if (PointsText != null)
            PointsText.text = GameController.controller.Points.ToString("F0");
    }

    #region BotoesUI
    
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
        Time.timeScale = 1.0f;
    }
    public void BotaoJogar()
    {
        SceneManager.LoadScene(1);
        GameController.controller.ResumeGameTime();

    }

    public void BotaoSair()
    {
        Application.Quit();
    }

    public void BotaoLobby()
    {
        SceneManager.LoadScene(4);
    }

    public void BotaoConfigON()
    {
        OptionsPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void BotaoPauseON()
    {
        audioSource.Pause();
        PausePanel.SetActive(true);
        isPause = true;
        Time.timeScale = 0;
    }
    public void BotaoPauseOFF()
    {
        audioSource.UnPause();
        PausePanel.SetActive(false);
        isPause = false;

        if (Time.timeScale < 1 & Time.timeScale > 0)
            return;

        Time.timeScale = 1;

        GameController.controller.ResumeGameTime();


    }
    public void BotaoAudioON()
    {
        AudioPanel.SetActive(true);
    }
    public void BotaoAudioOFF()
    {
        AudioPanel.SetActive(false);

    }
    public void BotaoCredtisON()
    {
        currentCredit = 0;
        CreditsPanel[currentCredit].SetActive(true);
    }
    public void BotaoCreditsOFF()
    {
        currentCredit = 0;
        CreditsPanel[currentCredit].SetActive(false);
    }

    public void BotaoCredtisNext()
    {
        currentCredit += 1;
        CreditsPanel[currentCredit].SetActive(true);
    }
    public void BotaoCreditsBefore()
    {
        
        CreditsPanel[currentCredit].SetActive(false);
        currentCredit -= 1;
    }





    public void BotaoTutorialON()
    {
        currentTutorial = 0;
        TutorialPanel[currentTutorial].SetActive(true);
    }
    public void BotaoTutorialOFF()
    {
        currentTutorial = 0;
        TutorialPanel[currentTutorial].SetActive(false);
    }

    public void BotaoTutorialNext()
    {
        currentTutorial += 1;
        TutorialPanel[currentTutorial].SetActive(true);
    }
    public void BotaoTutorialBefore()
    {

        TutorialPanel[currentTutorial].SetActive(false);
        currentTutorial -= 1;
    }





    public void BotaoConfigOFF()
    {
        OptionsPanel.SetActive(false);
        
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    #endregion BotoesUI

    #region CheckPonints


    public void ShowDeathPanel()
    {
        DeathPanel.SetActive(true);

        Time.timeScale = 0;
    }

    public void ShowVictoryPanel()
    {
        VictoryPanel.SetActive(true);

        Time.timeScale = 0;
    }



    public void RespawnCurrentCheckpoint()
    {
        DeathPanel.SetActive(false);
        VictoryPanel.SetActive(false);
        PausePanel.SetActive(false);
        CheckpointPanel.SetActive(false);

        GameController.controller.RespawnPlayer();

        Time.timeScale = 1;
        GameController.controller.ResumeGameTime();

    }

 


    public void OpenCheckpointMenu()
    {
        GameController.controller.IsOnCheckpointsPanel = true;
        CheckpointPanel.SetActive(true);
    }

    public void CloseCheckpointMenu()
    {
        GameController.controller.IsOnCheckpointsPanel = false;

        CheckpointPanel.SetActive(false);
    }


    public void TravelButton()
    {
        DeathPanel.SetActive(false);
        VictoryPanel.SetActive(false);
        PausePanel.SetActive(false);
        CheckpointPanel.SetActive(false);

        


        GameController.controller.playerRef.ResetPlayer();
        GameController.controller.ResumeGameTime();
        CloseCheckpointMenu();
    }

    public void NumberChackpoint(int index)
    {

        for (int i = 0; i < Haeds.Length; i++)
        {
            if (Haeds[i]) Haeds[i].SetActive(false);


        }

        for (int i = 0; i < Haeds.Length; i ++)
        {
            if(Haeds[index]) Haeds[index].SetActive(true);

            
        }
       

        GameController.controller.TravelToCheckpoint(index);
    }

    public void MapsUnlocked()
    {
        for (int i = 0; i < Maps.Length; i++)
        {
            if (Maps[i]) Maps[i].SetActive(false);


        }

        for (int i = 0; i < Maps.Length; i++)
        {
            if (GameController.controller.UnlockedCheckpoints[i] == true) Maps[i].SetActive(true);


        }
    }



    #endregion CheckPoints

    #region ChainsUI

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

    #endregion ChainsUI
}
