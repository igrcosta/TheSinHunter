using UnityEngine;

public class InputHandler : MonoBehaviour
{
    float timeNow, LastTapTime;

    int TapCount;

    Vector2 startTouch;


    private void Update()
    {
        DetectSlides();
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
                        else
                        {
                            //nada;
                        }

                    }
                    else
                    {
                        if (delta.y > 0)
                        {
                            GameController.controller.playerRef.MobileJumping();
                        }

                        else
                        {
                            GameController.controller.playerRef.MobileDescendingLanes();
                        }
                    }

                }




            }



        }




    }
}
