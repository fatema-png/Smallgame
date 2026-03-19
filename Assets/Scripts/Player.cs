using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 20f;
    public HeartUI heartUI;
    public GameOverUI gameOverUI;
    public int coins = 0;

    private int currentLives = 3;
    private float timer = 0f;
    private bool gameEnded = false;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;

    private int jumpCount = 0;
    private int maxJumps = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentLives = 3;
        if (heartUI != null)
            heartUI.UpdateHearts(currentLives);
    }

    void Update()
    {
        if (!gameEnded)
            timer += Time.deltaTime;

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
        }

        if (rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 1.1f * Time.deltaTime;
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 0.9f * Time.deltaTime;

        if (transform.position.y < -10f)
            TakeFallDamage();

        SetAnimation(moveInput);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }

        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            isGrounded = true;
            jumpCount = 0;
            transform.SetParent(collision.transform);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            float yOffset = transform.position.y - collision.transform.position.y;
            if (yOffset > 0.5f && rb.linearVelocity.y <= 0)
            {
                Destroy(collision.gameObject);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.8f);
                jumpCount = 0;
            }
            else if (!isInvincible)
            {
                StartCoroutine(TakeDamage());
            }
        }

        if (collision.gameObject.CompareTag("Damage") && !isInvincible)
            StartCoroutine(TakeDamage());
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;

        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            isGrounded = false;
            transform.SetParent(null);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coins++;
            other.GetComponent<Coin>().Collect();
        }
    }

    public void GetHit()
    {
        if (!isInvincible)
            StartCoroutine(TakeDamage());
    }

    void TakeFallDamage()
    {
        if (isInvincible) return;
        StartCoroutine(TakeDamage(true));
    }

    IEnumerator TakeDamage(bool isFall = false)
    {
        isInvincible = true;
        currentLives--;

        if (heartUI != null)
            heartUI.UpdateHearts(currentLives);

        if (currentLives <= 0)
        {
            GameOver();
            yield break;
        }

        if (isFall)
        {
            transform.position = new Vector3(0f, 0f, 0f);
            rb.linearVelocity = Vector2.zero;
            jumpCount = 0;
        }

        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        isInvincible = false;
    }

    void GameOver()
    {
        gameEnded = true;
        if (gameOverUI != null)
            gameOverUI.ShowGameOver(coins, timer);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetAnimation(float moveInput)
    {
        if (animator == null) return;

        if (isGrounded)
        {
            if (moveInput == 0 && animator.HasState(0, Animator.StringToHash("Player_Idle")))
                animator.Play("Player_Idle");
            else if (moveInput != 0 && animator.HasState(0, Animator.StringToHash("Run")))
                animator.Play("Run");
        }
        else
        {
            if (rb.linearVelocity.y > 0 && animator.HasState(0, Animator.StringToHash("Jump1")))
                animator.Play("Jump1");
            else if (animator.HasState(0, Animator.StringToHash("Jump2")))
                animator.Play("Jump2");
        }
    }
}
