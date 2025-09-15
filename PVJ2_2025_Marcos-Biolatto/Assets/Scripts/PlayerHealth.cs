using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float drainRate = 3f; // vida por segundo
    public bool isDraining = false;

    public HealthBarController healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    void Update()
    {
        if (isDraining)
        {
            currentHealth -= drainRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0f;
    }

    public void StopDrain()
    {
        isDraining = false;
    }

    public void StartDrain()
    {
        isDraining = true;
    }
}
