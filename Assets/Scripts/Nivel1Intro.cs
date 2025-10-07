using UnityEngine;
using TMPro;
using System.Collections;

public class Nivel1Intro : MonoBehaviour

{
    private TMP_Text texto;
    private GameObject player;
    private GameObject enemy;

    void Start()
    {
        Debug.Log("Nivel1Intro está activo en escena.");

        texto = GameObject.Find("TextoTutorialNivel1")?.GetComponent<TMP_Text>();
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");

        if (texto == null || player == null || enemy == null)
        {
            Debug.LogError("Faltan objetos. Verificá nombres: TextoTutorialNivel1, Player, Enemy.");
            return;
        }

        // Bloquear movimiento
        player.SetActive(false);
        enemy.SetActive(false);

        StartCoroutine(MostrarInstrucciones());
    }

    IEnumerator MostrarInstrucciones()
    {
        texto.gameObject.SetActive(true);
        texto.text = "En el nivel 1 ahora cuentas con la barra azul de stamina que sirve para golpear, con click izquierdo puedes atacar";
        yield return new WaitForSeconds(5f);

        texto.text = "Debes derrotar al enemigo con 5 golpes para ganar, presiona Esc para reintentarlo";
        yield return new WaitForSeconds(5f);

        texto.gameObject.SetActive(false);

        // Activar movimiento
        player.SetActive(true);
        enemy.SetActive(true);
    }

    public void MostrarVictoria()
    {
        if (texto == null)
        {
            Debug.LogError("Referencia a TMP_Text no está asignada.");
            return;
        }

        texto.gameObject.SetActive(true);
        texto.enabled = true;
        texto.text = "¡Felicidades venciste a tu enemigo! NIVEL SUPERADO, presiona Esc para reintentar";

        Debug.Log("Texto de victoria mostrado correctamente.");
    }



    public void MostrarDerrota()
    {
        texto.gameObject.SetActive(true);
        texto.text = "Tu enemigo te derrotó. Presiona Esc para reintentar";
    }
}
