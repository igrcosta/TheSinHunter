using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] GameObject OptionsPanel;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject AudioPanel;
    [SerializeField] GameObject[] CreditsPanel;
    [SerializeField] GameObject DeathPanel;
    [SerializeField] GameObject CheckpointPanel;



    int currentCredit = 0;
    public GameObject ChainsAim;
    public TMPro.TextMeshProUGUI DistanceText;
    public TMPro.TextMeshProUGUI PointsText;
    public bool isPause = false;

    [Header("Som")]
    public AudioSource audioSource;

    void Start()
    {
        GameController.controller.UIManager = this;

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

    public void RespawnCurrentCheckpoint()
    {
        DeathPanel.SetActive(false);
        PausePanel.SetActive(false);
        CheckpointPanel.SetActive(false);

        GameController.controller.RespawnPlayer();

        Time.timeScale = 1;
        GameController.controller.ResumeGameTime();

    }
    public void OpenCheckpointMenu()
    {
        
        CheckpointPanel.SetActive(true);
    }

    public void CloseCheckpointMenu()
    {
        
        CheckpointPanel.SetActive(false);
    }


    public void TravelButton(int index)
    {
        DeathPanel.SetActive(false);
        PausePanel.SetActive(false);
        CheckpointPanel.SetActive(false);

        GameController.controller.TravelToCheckpoint(index);

        GameController.controller.playerRef.ResetPlayer();
        GameController.controller.ResumeGameTime();
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
