using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; //  Importar para cambiar de escena

public class TutorialTextManager : MonoBehaviour
{
    public TMP_Text instructionText;
    public PlayerHealth playerHealth;
    public PlayerJump playerJump;

    private bool hudActive = false;
    private bool puedeAvanzar = false; // ← agregado
    private bool desafioTerminado = false; // ← agregado

    void Start()
    {
        instructionText.text = "Controles:\nA y D para moverse\nSpace para saltar";
        instructionText.gameObject.SetActive(true);

        Invoke(nameof(ShowChallengeIntro), 5f);
    }

    void ShowChallengeIntro()
    {
        instructionText.text = $"Haz {playerJump.requiredJumps} saltos en las plataformas en {playerJump.challengeTime} segundos antes de que se agote la vida";

        playerHealth.StartDrain();
        playerJump.StartChallenge();
        playerJump.enabled = true;

        Invoke(nameof(ActivateHUD), 2f);
    }

    void ActivateHUD()
    {
        hudActive = true;
    }

    void Update()
    {
        if (desafioTerminado && Input.GetKeyDown(KeyCode.Q)) // ← solo si terminó el desafío
        {
            SceneManager.LoadScene("Nivel1");
            return;
        }

        if (!hudActive || playerJump == null || instructionText == null || desafioTerminado) return;

        float timeRemaining = Mathf.Max(0f, playerJump.challengeTime - playerJump.ElapsedTime());
        instructionText.text = $"Saltos: {playerJump.jumpCount}/{playerJump.requiredJumps}\nTiempo: {timeRemaining:F1}s";
    }


    public void OnChallengeSuccess()
    {
        hudActive = false;
        desafioTerminado = true; // ← agregado
        instructionText.text = "¡Nivel superado!\nPresiona Q para avanzar al siguiente nivel";
        playerHealth.StopDrain();
    }

    public void OnChallengeFail()
    {
        hudActive = false;
        instructionText.text = "¡Has muerto!\nPresiona ESC para reintentar";
        playerHealth.StopDrain();

        Animator anim = playerJump.GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("isDead");
    }
}
