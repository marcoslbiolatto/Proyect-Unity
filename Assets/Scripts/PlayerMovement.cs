using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // A�adido: referencia al Animator
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Aplicar movimiento horizontal
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y); // Corregido: era linearVelocity

        // Flip horizontal del sprite sin alterar la escala original
        Vector3 escalaActual = transform.localScale;

        if (moveInput > 0)
            transform.localScale = new Vector3(Mathf.Abs(escalaActual.x), escalaActual.y, escalaActual.z); // derecha
        else if (moveInput < 0)
            transform.localScale = new Vector3(-Mathf.Abs(escalaActual.x), escalaActual.y, escalaActual.z); // izquierda

        // Actualizar par�metro de animaci�n
        anim.SetBool("isRunning", Mathf.Abs(moveInput) > 0); // A�adido: activa Run si hay movimiento
    }
}
