using UnityEngine;

public class BritlleWall : MonoBehaviour
{

    [SerializeField] private GameObject BritlleWallFX;
    void Start()
    {

    }

    void Update()
    {

    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        
        //if (CompareTag("Player"))
        //{

            if (!GameController.controller.SuperDashUnlocked)
                return;

            if (GameController.controller.playerRef.isDashing)
            {
                Instantiate(BritlleWallFX, this.gameObject.transform.position, Quaternion.identity);

                DestroyWall();
            }
        //}


    }

    public void DestroyWall()
    {
        gameObject.SetActive(false);
    }

}
