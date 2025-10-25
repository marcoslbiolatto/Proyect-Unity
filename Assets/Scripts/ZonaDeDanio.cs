using UnityEngine;

public class ZonaDeDanio : MonoBehaviour
{
    public int danioPorSegundo = 5;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.RecibeDanioContinuo(transform.position, danioPorSegundo);
            }
        }
    }
}
