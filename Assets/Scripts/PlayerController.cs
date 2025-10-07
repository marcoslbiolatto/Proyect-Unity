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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("RecibeDanio", recibiendoDanio);
        if (Input.GetKeyDown(KeyCode.Mouse0)) // ataque
        {
            anim.SetTrigger("isAttacking");

            // Detectar al Enemy cerca
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f); // radio de ataque

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
            // Movimiento normal o lógica de ataque
            // Asegurate de que el ataque se active solo si no está recibiendo daño
            if (Input.GetKeyDown(KeyCode.Space)) // ejemplo de ataque
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
            }
            else
            {
                StartCoroutine(ResetDanio());
            }
        }
    }

    IEnumerator ResetDanio()
    {
        yield return new WaitForSeconds(0.5f); // duración de la animación de Hurt
        DesactivaDanio();
    }

    public void DesactivaDanio()
    {
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero;
    }
}
