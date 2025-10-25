using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float knockbackForce = 5f;
    private bool recibiendoDanio = false;
    private bool isDead = false;

    private Rigidbody2D rb;
    private Animator anim;

    public PlayerHealth playerHealth; // Asignar desde el Inspector

    private Nivel1Intro intro; // Referencia al controlador de mensajes

    [SerializeField] private AudioSource audioSourceMuerte;
    [SerializeField] private AudioClip sonidoMuerte;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        intro = GameObject.FindObjectOfType<Nivel1Intro>(); // Inicializar referencia
    }

    void Update()
    {
        anim.SetBool("RecibeDanio", recibiendoDanio);

        if (Input.GetKeyDown(KeyCode.Mouse0)) // ataque
        {
            anim.SetTrigger("isAttacking");

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    EnemyController enemy = hit.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        enemy.RecibeGolpe();
                    }
                }
            }
        }

        if (!recibiendoDanio && !isDead)
        {
            if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("SampleScene") && Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("isAttacking");
            }
        }
    }

    public void RecibeDanio(Vector2 origenDelGolpe, int cantidadDanio)
    {
        if (recibiendoDanio || isDead) return;

        recibiendoDanio = true;
        Debug.Log("¡Daño recibido! Cantidad: " + cantidadDanio);

        Vector2 direccionRebote = (transform.position - (Vector3)origenDelGolpe).normalized;
        direccionRebote.y = 1f;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direccionRebote * knockbackForce, ForceMode2D.Impulse);

        if (playerHealth != null)
        {
            playerHealth.currentHealth -= cantidadDanio;
            playerHealth.currentHealth = Mathf.Clamp(playerHealth.currentHealth, 0f, playerHealth.maxHealth);
            playerHealth.healthBar.SetHealth(playerHealth.currentHealth, playerHealth.maxHealth);

            if (playerHealth.IsDead())
            {
                isDead = true;
                anim.SetTrigger("isDead");
                rb.linearVelocity = Vector2.zero;

                if (audioSourceMuerte != null && sonidoMuerte != null)
                {
                    audioSourceMuerte.PlayOneShot(sonidoMuerte);
                }

                if (intro != null)
                {
                    intro.MostrarDerrota();
                }
            }
            else
            {
                StartCoroutine(ResetDanio());
            }
        }
    }

    public void RecibeDanioContinuo(Vector2 origenDelGolpe, int cantidadDanio)
    {
        if (isDead) return;

        Debug.Log("Daño continuo recibido: " + cantidadDanio);

        if (playerHealth != null)
        {
            playerHealth.currentHealth -= cantidadDanio;
            playerHealth.currentHealth = Mathf.Clamp(playerHealth.currentHealth, 0f, playerHealth.maxHealth);
            playerHealth.healthBar.SetHealth(playerHealth.currentHealth, playerHealth.maxHealth);

            if (playerHealth.IsDead())
            {
                isDead = true;
                anim.SetTrigger("isDead");
                rb.linearVelocity = Vector2.zero;

                if (audioSourceMuerte != null && sonidoMuerte != null)
                {
                    audioSourceMuerte.PlayOneShot(sonidoMuerte);
                }

                if (intro != null)
                {
                    intro.MostrarDerrota();
                }
            }
        }
    }


    IEnumerator ResetDanio()
    {
        yield return new WaitForSeconds(0.5f);
        DesactivaDanio();
    }

    public void DesactivaDanio()
    {
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero;
    }
}
