using UnityEngine;

public class ItemsScript : MonoBehaviour
{
    private Player Pr;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Pr = GameController.controller.playerRef;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if  (other.CompareTag("Player"))
        {
            

            switch (tag)
            {

                case "SuperDash":
                    {
                        GameController.controller.SuperDashUnlocked= true;
                        
                        break;
                    }
                case "DoubleJump":
                    {
                        GameController.controller.DoubleJumpUnlocked = true; 
                        break;
                    }
                case "SkySlash":
                    {
                        GameController.controller.SkySlashUnlocked = true;
                        break;
                    }
                case "Chains":
                    {
                        GameController.controller.ChainsUnlocked = true;
                        break;
                    }
                case "Key":
                {
                        Pr.KeyCollected = true;

                   break;
                }
            }

        }
    }
}
