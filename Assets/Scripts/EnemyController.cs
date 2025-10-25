using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRadius = 2.5f;
    public LayerMask playerLayer;

    [SerializeField] private int golpesRecibidos = 0;
    public int golpesParaMorir = 5;

    private bool isDead = false;
    private bool isHurting = false;

    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;

    private Nivel1Intro intro;

    public GameObject prefabBolaDeFuego;
    public Transform puntoDisparo;
    public float frecuenciaDisparo = 1f;
    public float velocidadProyectil = 5f;

    private float tiempoEntreDisparos;
    private float temporizadorDisparo;

    private static int enemigosGenerados = 0;
    public int cantidadMaxima = 5;
    public float intervaloGeneracion = 5f;

    public void AsignarJugador(Transform jugador)
    {
        player = jugador;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // No buscar al jugador en Start si puede estar desactivado
        intro = GameObject.FindObjectOfType<Nivel1Intro>();

        tiempoEntreDisparos = 1f / frecuenciaDisparo;

        if (SceneManager.GetActiveScene().name == "Nivel2")
        {
            InvokeRepeating("GenerarOtroEnemigo", 2f, intervaloGeneracion);
        }
        else
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        // 🔁 Buscar al jugador si aún no fue asignado
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log("✔ Player asignado dinámicamente");
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        if (isDead || isHurting)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 🔍 Calcular dirección hacia el jugador
        Vector2 direccion = (player.position - transform.position).normalized;
        float distancia = Vector2.Distance(transform.position, player.position);

        Debug.Log("Distancia al jugador: " + distancia);
        Debug.Log("Dirección hacia el jugador: " + direccion);

        // ✅ Si está dentro del radio de detección, moverse y atacar
        if (distancia < detectionRadius)
        {
            anim.SetBool("isAttacking", true);
            rb.linearVelocity = new Vector2(direccion.x * moveSpeed, rb.linearVelocity.y);

            // Orientación visual
            if (direccion.x > 0)
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            else if (direccion.x < 0)
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

            // Disparo
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

            intro?.MostrarVictoria();
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

    void Disparar()
    {
        GameObject bola = Instantiate(prefabBolaDeFuego, puntoDisparo.position, Quaternion.identity);
        BolaDeFuego script = bola.GetComponent<BolaDeFuego>();
        if (script != null)
        {
            script.velocidad = velocidadProyectil;
        }
    }

    void GenerarOtroEnemigo()
    {
        if (enemigosGenerados >= cantidadMaxima) return;

        Instantiate(gameObject, transform.position + Vector3.right * 2f, Quaternion.identity);
        enemigosGenerados++;
    }
}
