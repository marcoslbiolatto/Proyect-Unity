using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Añadido: referencia al Animator
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Aplicar movimiento horizontal
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); // Corregido: era linearVelocity

        // Flip horizontal del sprite sin alterar la escala original
        Vector3 escalaActual = transform.localScale;

        if (moveInput > 0)
            transform.localScale = new Vector3(Mathf.Abs(escalaActual.x), escalaActual.y, escalaActual.z); // derecha
        else if (moveInput < 0)
            transform.localScale = new Vector3(-Mathf.Abs(escalaActual.x), escalaActual.y, escalaActual.z); // izquierda

        // Actualizar parámetro de animación
        anim.SetBool("isRunning", Mathf.Abs(moveInput) > 0); // Añadido: activa Run si hay movimiento
    }
}
