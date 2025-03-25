using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIController : MonoBehaviour
{
    public void OnBackToMenuButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BackToMenu();
        }
        else
        {
            Debug.LogWarning("GameManager not found!");
            SceneManager.LoadScene("Menu");
        }
    }
}