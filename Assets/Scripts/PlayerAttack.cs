using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;

    public StaminaBarController staminaBar;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaCost = 20f;
    public float staminaRegenRate = 10f;

    public float staminaThresholdToAttack = 20f;
    private bool canAttack = true;

    public float attackRange = 1f; // 🔹 Distancia del golpe
    public LayerMask enemyLayer;   // 🔹 Asignar en el Inspector

    private Coroutine regenCoroutine;

    [SerializeField] private AudioSource audioSourceGolpe;
    [SerializeField] private AudioClip sonidoGolpe;


    void Start()
    {
        anim = GetComponent<Animator>();
        currentStamina = maxStamina;

        if (staminaBar != null)
        {
            staminaBar.SetStamina(currentStamina, maxStamina);
        }
        else
        {
            Debug.LogWarning("⚠️ StaminaBar no asignada en esta escena. Se omite visual.");
        }
    }

    void Update()
    {
        canAttack = currentStamina >= staminaThresholdToAttack;

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            anim.SetBool("isAttacking", true);
            if (audioSourceGolpe != null && sonidoGolpe != null)
            {
                audioSourceGolpe.PlayOneShot(sonidoGolpe);
            }

            currentStamina -= staminaCost;

            if (staminaBar != null)
            {
                staminaBar.SetStamina(currentStamina, maxStamina);
            }

            if (regenCoroutine != null)
                StopCoroutine(regenCoroutine);

            regenCoroutine = StartCoroutine(RegenerateStamina());

            // 🔍 Detectar enemigo en rango y aplicar golpe
            Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, enemyLayer);
            if (hit != null)
            {
                EnemyController ec = hit.GetComponent<EnemyController>();
                if (ec != null)
                {
                    ec.RecibeGolpe();
                }
            }
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }

    IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(1f);

        while (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);

            if (staminaBar != null)
            {
                staminaBar.SetStamina(currentStamina, maxStamina);
            }

            yield return null;
        }

        regenCoroutine = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
