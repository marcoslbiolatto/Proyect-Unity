using UnityEngine;

public class Cura : MonoBehaviour
{
    public float cantidadCura = 20f;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            PlayerController pc = other.collider.GetComponent<PlayerController>();
            if (pc != null && pc.playerHealth != null)
            {
                pc.playerHealth.currentHealth += cantidadCura;
                pc.playerHealth.currentHealth = Mathf.Clamp(pc.playerHealth.currentHealth, 0f, pc.playerHealth.maxHealth);
                pc.playerHealth.healthBar.SetHealth(pc.playerHealth.currentHealth, pc.playerHealth.maxHealth);

                gameObject.SetActive(false); // o Destroy(gameObject) si no usás pooling
            }
        }
    }
}
