using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRadius = 0.5f;
    public LayerMask playerLayer;

    [SerializeField] private int golpesRecibidos = 0;
    public int golpesParaMorir = 5;

    private bool isDead = false;
    private bool isHurting = false;

    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;

    private Nivel1Intro intro; // Referencia directa al controlador

    // 🔹 Disparo: variables públicas
    public GameObject prefabBolaDeFuego;
    public Transform puntoDisparo;
    public float frecuenciaDisparo = 1f;
    public float velocidadProyectil = 5f;

    // 🔹 Disparo: variables internas
    private float tiempoEntreDisparos;
    private float temporizadorDisparo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        intro = GameObject.FindObjectOfType<Nivel1Intro>(); // Guardar referencia

        // 🔹 Inicializar frecuencia de disparo
        tiempoEntreDisparos = 1f / frecuenciaDisparo;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("isHurt");
        }

        if (isDead || isHurting || player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            anim.SetBool("isAttacking", true);

            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

            if (direction.x > 0)
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            else if (direction.x < 0)
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

            // 🔹 Disparo: controlar temporizador
            temporizadorDisparo += Time.deltaTime;
            if (temporizadorDisparo >= tiempoEntreDisparos)
            {
                Disparar();
                temporizadorDisparo = 0f;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isAttacking", false);
        }
    }

    public void RecibeGolpe()
    {
        if (isDead || isHurting) return;

        intro?.ActualizarContadorGolpes(golpesRecibidos, golpesParaMorir);

        ProgressionManager.Instance?.RegistrarGolpe();

        golpesRecibidos++;
        Debug.Log("Golpes recibidos: " + golpesRecibidos);

        if (golpesRecibidos >= golpesParaMorir)
        {
            isDead = true;
            rb.linearVelocity = Vector2.zero;

            anim.SetBool("isAttacking", false);
            anim.ResetTrigger("isHurt");
            anim.SetTrigger("isDead");

            Debug.Log("Enemy ha muerto");

            if (intro != null)
            {
                intro.MostrarVictoria();
            }
        }
        else
        {
            StartCoroutine(AnimacionHurt());
        }
    }

    IEnumerator AnimacionHurt()
    {
        isHurting = true;
        anim.SetTrigger("isHurt");
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        anim.ResetTrigger("isHurt");
        anim.SetBool("isAttacking", false);
        isHurting = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || isHurting) return;

        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                Vector2 direccion = transform.position;
                pc.RecibeDanio(direccion, 5);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // 🔹 Método de disparo
    void Disparar()
    {
        GameObject bola = Instantiate(prefabBolaDeFuego, puntoDisparo.position, Quaternion.identity);
        BolaDeFuego script = bola.GetComponent<BolaDeFuego>();
        if (script != null)
        {
            script.velocidad = velocidadProyectil;
        }
    }
}
