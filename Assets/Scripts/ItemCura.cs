using UnityEngine;

public class ItemCura : MonoBehaviour
{
    public float cantidadCura = 20f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null && pc.playerHealth != null)
            {
                pc.playerHealth.currentHealth += cantidadCura;
                pc.playerHealth.currentHealth = Mathf.Clamp(pc.playerHealth.currentHealth, 0f, pc.playerHealth.maxHealth);
                pc.playerHealth.healthBar.SetHealth(pc.playerHealth.currentHealth, pc.playerHealth.maxHealth);

                Destroy(gameObject);
            }
        }
    }
}
