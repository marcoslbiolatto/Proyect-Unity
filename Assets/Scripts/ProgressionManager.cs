using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance { get; private set; }

    public ProgresionData datosProgresion;
    public int golpesActuales = 0;
    public bool victoriaRegistrada = false;

    private Nivel1Intro intro;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        intro = FindObjectOfType<Nivel1Intro>();
    }

    public void RegistrarGolpe()
    {
        if (victoriaRegistrada) return;

        golpesActuales++;

        intro?.ActualizarContadorGolpes(golpesActuales, datosProgresion.golpesParaVictoria);

        if (golpesActuales >= datosProgresion.golpesParaVictoria)
        {
            victoriaRegistrada = true;
            intro?.MostrarVictoria();
        }
    }
}
