using UnityEngine;

public class AvarezaScripot : MonoBehaviour
{
    private Rigidbody rb;
    private float WalkSpeed = 20f;
    [SerializeField] float Damage = 20f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position += Vector3.left * WalkSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing == false) 
        {
            GameController.controller.playerRef.Hit(Damage);
            Debug.Log("AVAREZA DEU DANO");
        }
        else if (other.CompareTag("Player") && GameController.controller.playerRef.isDashing)
        {
            Destroy(gameObject);
        }
    }
}
