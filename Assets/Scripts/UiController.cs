    using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject Options;
    
    public GameObject ChainsAim;
    public TMPro.TextMeshProUGUI TextoDistancia;
    public TMPro.TextMeshProUGUI TextoPontos;


    private void Update()
    {
        MostrarTextosUI();
    }
    public void MostrarTextosUI()
    {
        TextoDistancia.text = GameController.controller.Distancia.ToString("F0") + " M";
        TextoPontos.text = GameController.controller.Pontos.ToString("F0");
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
