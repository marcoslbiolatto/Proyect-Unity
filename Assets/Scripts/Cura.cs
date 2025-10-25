using UnityEngine;

public class Cura : MonoBehaviour
{
    [SerializeField] private ParticleSystem particulasCura;

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

                if (particulasCura != null)
                {
                    particulasCura.transform.parent = null; // para que no se destruya con el objeto
                    particulasCura.Play();
                    Destroy(particulasCura.gameObject, 2f); // destruir después de reproducirse
                }

                gameObject.SetActive(false); // o Destroy(gameObject) si no usás pooling
            }
        }
    }
}
