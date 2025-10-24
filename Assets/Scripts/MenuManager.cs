using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("SampleScene"); // Cambiá "SampleScene"
    }

    public void Salir()
    {
        Application.Quit(); // Cierra el juego en el build

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Detiene el modo Play en el editor
#endif
    }
}
