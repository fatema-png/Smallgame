using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    public int maxLives = 3;
    public int currentLives;
    public HeartUI heartUI;

    // Invincibility frames
    public float invincibleTime = 1.0f;
    private bool isInvincible = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentLives = maxLives;
        heartUI.UpdateHearts(currentLives);
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

        currentLives--;
        heartUI.UpdateHearts(currentLives);

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
        }
    }

    System.Collections.IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        // Reload the scene (or load a Game Over scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}