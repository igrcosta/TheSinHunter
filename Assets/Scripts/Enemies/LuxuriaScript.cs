using UnityEngine;

public class LuxuriaScript : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] float PointsGuiven = 250f;

    [Header("Mechanics")]
    [SerializeField] float Damage = 15f;
    [SerializeField] float pushDistanceChains = 5f;
    [SerializeField] float pushForceDefault = 10f;

    private Rigidbody rb;
    private GameObject shield;

    void Awake() => rb = GetComponent<Rigidbody>();

    void Start()
    {
        if (transform.childCount > 0)
            shield = transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = GameController.controller.playerRef;

            if (shield != null)
            {
                if (player.isDashing || player.ExplosionState)
                {
                    player.rb.position = new Vector3(transform.position.x - 2f, player.rb.position.y, player.rb.position.z);

                    HandleShieldBreak(player);
                }
                else
                {
                    player.Hit(Damage);
                }
            }
            else
            {
                if (player.isDashing || player.ExplosionState)
                {
                    HandleDeath(player);
                }
                else
                {
                    player.Hit(Damage);
                }
            }
        }
    }

    public void Death()
    {
        GameController.controller.AddPoints(PointsGuiven);
        //Instantiate(deathFX, this.gameObject.transform.position, Quaternion.identity);
        gameObject.SetActive(false);

    }

    void HandleShieldBreak(Player player)
    {
        Destroy(shield);
        shield = null;

        bool isUsingChains = player.ActualWeapon == Player.WeaponTypes.LuxuryChains;

        player.EnableDash();
        player.rb.linearVelocity = Vector3.zero;
        player.rb.angularVelocity = Vector3.zero;
        // Reset total de inércia para impedir que ele continue o movimento do dash

        if (isUsingChains)
        {
            rb.MovePosition(rb.position + Vector3.right * pushDistanceChains);
            player.rb.MovePosition(player.rb.position + Vector3.left * 1.5f);
            // Correntes: Empurrão seco
        }
        else
        {
            rb.AddForce(new Vector3(pushForceDefault, 2f, 0f), ForceMode.Impulse);
            // Dash Default: Impacto físico
            // Jogamos a luxúria um pouco pra frente e pra cima (firula)

            // Jogamos o player um pouco pra trás
            player.rb.AddForce(new Vector3(-pushForceDefault / 1.5f, 3f, 0f), ForceMode.Impulse);
        }

        CancelInvoke("RestoreSpeed");
        Invoke("RestoreSpeed", 0.15f);
        // Delay curto para o jogador ver que bateu, antes de voltar a correr
    }

    void RestoreSpeed()
    {
        Player player = GameController.controller.playerRef;
        if (player != null)
            player.rb.linearVelocity = new Vector3(player.Speed, player.rb.linearVelocity.y, 0);
    }

    void HandleDeath(Player player)
    {
        if (player.ExplosionState) player.ContinuousRageExplosion();
        else player.FinishDash();

        GameController.controller.AddPoints(PointsGuiven);

        Death();


    }
}

//eu tentei muito deixar algo mais fluido com movetowards ou lerp, perguntei até pras IAS, mas física dá mtos conflitos...