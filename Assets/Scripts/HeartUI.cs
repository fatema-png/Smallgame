using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    // Drag your 3 heart Image objects here
    public Image[] hearts;

    public void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // Simply show or hide each heart
            hearts[i].gameObject.SetActive(i < currentLives);
        }
    }
}