using UnityEngine;

public class ItemsScript : MonoBehaviour
{
    private Player player;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameController.controller.playerRef;
        animator = GetComponent<Animator>();
        animator.Play("ChainsAnimation");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.name == "ChainsItem")
            {
                player.SetActualWeapon("LuxuryChains");
            }
            else if (gameObject.name == "DefaultItem")
            {
                player.SetActualWeapon("Default");
            }
            else if (gameObject.name == "RageItem")
            {
                player.SetActualWeapon("RageBlade");
            }
        }
    }
}
