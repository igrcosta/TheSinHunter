using UnityEngine;

public class InputHandler : MonoBehaviour
{
    float timeNow, LastTapTime;

    int TapCount;

    Vector2 startTouch;


    private void Update()
    {
        DetectSlides();//Detecta os slides na tela e chama as funcoes de acordo
        HandleInputKeyboard(); //Detecta os inputs do teclado e chama as funcoes de acordo
    }

    void DetectSlides()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                startTouch = t.position;
            }
            else if (t.phase == TouchPhase.Ended)
            {
                Vector2 delta = t.position - startTouch;

                if (delta.magnitude > 100)
                {
                    if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    {
                        if (delta.x > 0)
                        {
                            GameController.controller.playerRef.BeginDash();
                        }

                    }
                    else
                    {
                        if (delta.y > 0)
                        {
                            GameController.controller.playerRef.JumpingMethod();
                        }

                        else
                        {
                            GameController.controller.playerRef.DescendingLanes();
                        }
                    }

                }
                else
                {
                    if(Time.timeScale == 1)
                    GameController.controller.playerRef.BeginDash();

                }

            }
        }
    }

    #region KeyBoard

    void HandleInputKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) GameController.controller.playerRef.JumpingMethod(); //Pulo
        if (Input.GetKeyDown(KeyCode.DownArrow)) GameController.controller.playerRef.DescendingLanes(); //Dash para baixo
        if (Input.GetKeyDown(KeyCode.RightArrow)) GameController.controller.playerRef.BeginDash(); //Dash
        if (Input.GetKeyDown(KeyCode.A)) GameController.controller.playerRef.BeginSlash(); //Dash
        if (Input.GetKeyDown(KeyCode.D)) GameController.controller.playerRef.SetActualWeapon("LuxuryChains"); //Muda para Correntes
        if (Input.GetKeyDown(KeyCode.S)) GameController.controller.playerRef.SetActualWeapon("Default"); //Muda para Default
        if (Input.GetKeyDown(KeyCode.W)) GameController.controller.playerRef.SetActualWeapon("RageBlade"); // Pulo duplo
    }


    #endregion KeyBoard

}

