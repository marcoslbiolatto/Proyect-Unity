using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Activar animación de ataque con clic izquierdo
        if (Input.GetMouseButtonDown(0))
            anim.SetBool("isAttacking", true);
        else
            anim.SetBool("isAttacking", false);
    }
}
