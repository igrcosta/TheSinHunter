using UnityEngine;

public class InputHandler : MonoBehaviour
{
    float timeNow, LastTapTime;

    int TapCount;

    Vector2 startTouch;

    Player Pref;
    [SerializeField] SlowCollider slow;
    [SerializeField] TimeCollider time;

    

    public bool TimeCinematic = false;
    public bool SlowCinematic = false;

    private void Start()
    {
        Pref = GameController.controller.playerRef;
        
        
    }
    private void Update()
    {
        DetectSlides();//Detecta os slides na tela e chama as funcoes de acordo
        HandleInputKeyboard(); //Detecta os inputs do teclado e chama as funcoes de acordo
    }

    void DetectSlides()
    {
        if (GameController.controller.UIManager.isPause)
            return;

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
                            Pref.BeginDash();
                            
                        }

                    }
                    else
                    {
                        if (delta.y > 0)
                        {

                            if (!Pref.CanJump && Pref.CanDoubleJump)
                            {
                                Pref.DoubleJump();
                            }
                            else
                            {
                                Pref.JumpingMethod();
                            }

                        }

                        else
                        {
                            
                            Pref.DescendingLanes();
                        }
                    }

                }
                else
                {
                        Pref.BeginDash();
                        SlowCinematic = false;
                }

            }
        }
        if (Input.touchCount == 3)
        {
            GameController.controller.Cheating = true;
        }
    }

    #region KeyBoard

    void HandleInputKeyboard()
    {
        if (Pref.CanDoubleJump && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Pref.DoubleJump();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && !Pref.IsDoubleJumping) Pref.JumpingMethod(); //Pulo
        
        if (Input.GetKeyDown(KeyCode.DownArrow)) Pref.DescendingLanes(); //Dash para baixo
        if (Input.GetKeyDown(KeyCode.RightArrow)) Pref.BeginDash(); //Dash
        if (Input.GetKeyDown(KeyCode.A)) Pref.BeginSlash(); //Dash
        if (Input.GetKeyDown(KeyCode.D)) Pref.SetActualWeapon("LuxuryChains"); //Muda para Correntes
        if (Input.GetKeyDown(KeyCode.S)) Pref.SetActualWeapon("Default"); //Muda para Default
        if (Input.GetKeyDown(KeyCode.W)) Pref.SetActualWeapon("RageBlade"); // Pulo duplo

        if (SlowCinematic) // Apenas roda se estiver em uma cinematic
        {
            if (Input.GetKeyDown(KeyCode.K)) slow.AcelleratePlayer();// Acelera o player a sua speed normal
        }

        if (SlowCinematic) // Apenas roda se estiver em uma cinematic
        {
            if (Input.GetKeyDown(KeyCode.Space))SlowCinematic = false ; //Retoma o tempo
            if (Input.GetKeyDown(KeyCode.RightArrow)) SlowCinematic = false;
        }
        if (Input.GetKeyDown(KeyCode.C)) GameController.controller.CheatMode();
        if (Input.GetKeyDown(KeyCode.V)) GameController.controller.NormalMode();

    }


    #endregion KeyBoard

}

