using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // ✅ Importar para cambiar de escena

public class TutorialTextManager : MonoBehaviour
{
    public TMP_Text instructionText;
    public PlayerHealth playerHealth;
    public PlayerJump playerJump;

    private bool hudActive = false;

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
        if (!hudActive || playerJump == null || instructionText == null) return;

        float timeRemaining = Mathf.Max(0f, playerJump.challengeTime - playerJump.ElapsedTime());
        instructionText.text = $"Saltos: {playerJump.jumpCount}/{playerJump.requiredJumps}\nTiempo: {timeRemaining:F1}s";
    }

    public void OnChallengeSuccess()
    {
        hudActive = false;
        instructionText.text = "¡Nivel superado!\nPresiona ESC para reintentar";
        playerHealth.StopDrain();

        // ✅ Transición automática a Nivel1
        SceneManager.LoadScene("Nivel1");
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
