using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] GameObject Options;
    public GameObject ChainsAim;
    public TMPro.TextMeshProUGUI DistanceText;
    public TMPro.TextMeshProUGUI PointsText;

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
    #endregion BotoesUI

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
