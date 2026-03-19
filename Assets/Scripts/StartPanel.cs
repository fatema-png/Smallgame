using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour
{
    public GameObject startPanel;

    void Start()
    {
        startPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowStartPanel()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}