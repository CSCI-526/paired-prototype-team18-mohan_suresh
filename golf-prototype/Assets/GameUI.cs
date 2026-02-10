using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text ballsRemainingText;
    [SerializeField] private Text ballSizeText;
    [SerializeField] private Text instructionsText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);
            
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateBallsRemaining(5);
        UpdateBallSize(BallSize.Normal);

        if (instructionsText != null)
        {
            instructionsText.text = "A/D: Rotate Aim | SPACE: Charge Power (press again to shoot)";
        }
    }

    public void UpdateBallsRemaining(int balls)
    {
        if (ballsRemainingText != null)
        {
            ballsRemainingText.text = "Balls: " + balls + "/5";
        }
    }

    public void UpdateBallSize(BallSize size)
    {
        if (ballSizeText != null)
        {
            ballSizeText.text = "Ball Size: " + size.ToString();
        }
    }

    public void ShowWin()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }
    
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
