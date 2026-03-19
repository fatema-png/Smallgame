using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timeText;
    public GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(int coins, float time)
    {
        gameOverPanel.SetActive(true);
        coinsText.text = "Coins: " + coins;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timeText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        Time.timeScale = 0f;
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}