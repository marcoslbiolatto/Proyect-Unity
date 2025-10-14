using UnityEngine;

public class BolaDeFuego : MonoBehaviour
{
    public float velocidad = 5f;
    public float daño = 10f;

    void Start()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        Vector2 direccion = Vector2.left; // dirección por defecto

        if (jugador != null)
        {
            direccion = (jugador.transform.position - transform.position).normalized;
        }

        GetComponent<Rigidbody2D>().velocity = direccion * velocidad;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                Vector2 origen = transform.position;
                pc.RecibeDanio(origen, (int)daño);
            }

            Destroy(gameObject);
        }
    }
}
