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
                        Pr.SuperDashUnlocked= true;
                        break;
                    }
                case "DoubleJump":
                    {
                        Pr.DoubleJumpUnlocked = true; 
                        break;
                    }
                case "SkySlash":
                    {
                        Pr.SkySlashUnlocked = true;
                        break;
                    }
                case "Chains":
                    {
                        Pr.ChainsUnlocked = true;
                        break;
                    }
            }

        }
    }
}
