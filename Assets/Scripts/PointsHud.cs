using UnityEngine;
using TMPro;

public class PointsHud : MonoBehaviour
{

    public TMP_Text Pontos;

    // Update is called once per frame
    void Update()
    {
        Pontos.text = GameController.controller.points.ToString();
    }
}
