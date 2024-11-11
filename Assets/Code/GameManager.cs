using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int maxScore = 50;
    private TextMeshProUGUI gameOverText;
    private bool isGameOver = false;

    void Start()
    {
        gameOverText = GameObject.Find("GameOverScreen").GetComponent<TextMeshProUGUI>();
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isGameOver)
        {
            // When the game ends display the game over text and the esc key instructions
            gameOverText.gameObject.SetActive(true);

            // Check for the ESC key to restart the game
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                RestartGame();
            }
        }
    }

    // Call this method when the game is over to freeze the game
    public void GameOver()
    {
        isGameOver = true;
        // Freeze the game
        Time.timeScale = 0;
    }

    // Restart the game
    void RestartGame()
    {
        isGameOver = false;
        gameOverText.gameObject.SetActive(false);

        // Reset game state here (e.g., score, player position, etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Unfreeze the game and reset the score
        Time.timeScale = 1;
        ScoreKeeper.ResetScore();
    }
}