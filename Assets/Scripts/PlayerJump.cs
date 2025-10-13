using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 6f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;

    public int jumpCount { get; private set; } = 0;
    public int requiredJumps = 15;
    public float challengeTime = 30f;

    private float timer;
    private bool challengeActive = false;
    private bool challengeEnded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Detección combinada: velocidad vertical + contacto físico
        bool touchingGround = Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.1f), 0f, groundLayer);
        bool lowVerticalSpeed = Mathf.Abs(rb.linearVelocity.y) < 0.01f;
        isGrounded = lowVerticalSpeed || touchingGround;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (challengeActive && !challengeEnded)
            {
                jumpCount++;
            }
        }

        if (challengeActive && !challengeEnded)
        {
            timer -= Time.deltaTime;

            if (jumpCount >= requiredJumps)
            {
                challengeEnded = true;
                challengeActive = false;
                FindObjectOfType<TutorialTextManager>().OnChallengeSuccess();
            }
            else if (timer <= 0f)
            {
                challengeEnded = true;
                challengeActive = false;

                if (anim != null)
                    anim.SetTrigger("isDead");

                FindObjectOfType<TutorialTextManager>().OnChallengeFail();
            }
        }

        // 🔁 Reinicio con ESC después de terminar
        if (challengeEnded && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void StartChallenge()
    {
        jumpCount = 0;
        timer = challengeTime;
        challengeActive = true;
        challengeEnded = false;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public float ElapsedTime()
    {
        return challengeTime - timer;
    }
}
