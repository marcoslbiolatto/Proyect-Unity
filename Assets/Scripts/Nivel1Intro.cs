using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Nivel1Intro : MonoBehaviour
{
    private TMP_Text texto;
    private GameObject player;
    private GameObject enemy;
    private GameObject generadorCuras;

    void Start()
    {
        Debug.Log("Nivel1Intro está activo en escena.");

        texto = GameObject.Find("TextoTutorialNivel1")?.GetComponent<TMP_Text>();
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        generadorCuras = GameObject.Find("GeneradorCuras");
        if (generadorCuras != null) generadorCuras.SetActive(false);

        if (texto == null || player == null || enemy == null)
        {
            Debug.LogError("Faltan objetos. Verificá nombres: TextoTutorialNivel1, Player, Enemy.");
            return;
        }

        texto.gameObject.SetActive(true);

        player.SetActive(false);
        enemy.SetActive(false);

        StartCoroutine(MostrarInstrucciones());
    }

    IEnumerator MostrarInstrucciones()
    {
        texto.text = "En el nivel 1 ahora cuentas con la barra azul de stamina que sirve para golpear. Con click izquierdo puedes atacar.";
        yield return new WaitForSeconds(5f);

        texto.text = "ten cuidado con la particulas que hacen danio.";
        yield return new WaitForSeconds(5f);



        texto.text = "Debes derrotar al enemigo con 5 golpes para ganar.";
        yield return new WaitForSeconds(5f);

        texto.text = "";

        player.SetActive(true);
        enemy.SetActive(true);
        if (generadorCuras != null) generadorCuras.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Nivel1"); // Reinicia el nivel actual
        }
    }

    public void MostrarVictoria()
    {
        if (texto == null) return;

        texto.text = "¡Felicidades, venciste a tu enemigo! Presiona Esc para volver a intentarlo.";
        Debug.Log("Texto de victoria mostrado correctamente.");
    }

    public void MostrarDerrota()
    {
        if (texto == null) return;

        texto.text = "Tu enemigo te derrotó. Presiona Esc para reintentar.";
        Debug.Log("Texto de derrota mostrado correctamente.");
    }

    public void ActualizarContadorGolpes(int actuales, int maximos)
    {
        if (texto == null) return;

        texto.text = "Golpes: " + actuales + " / " + maximos;
    }
}
