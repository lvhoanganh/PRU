using UnityEngine;

public class PlayAgain : MonoBehaviour
{
    public void OnPlayAgainButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
        else
        {
            Debug.LogWarning("GameManager not found!");
        }
    }
}