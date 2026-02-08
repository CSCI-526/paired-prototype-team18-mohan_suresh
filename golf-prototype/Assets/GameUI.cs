using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text strokesText;
    [SerializeField] private Text ballSizeText;
    [SerializeField] private Text instructionsText;
    [SerializeField] private GameObject winPanel;

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        UpdateStrokes(0);
        UpdateBallSize(BallSize.Normal);

        if (instructionsText != null)
        {
            instructionsText.text = "A/D: Rotate Aim | SPACE: Charge Power (press again to shoot)";
        }
    }

    public void UpdateStrokes(int strokes)
    {
        if (strokesText != null)
        {
            strokesText.text = "Strokes: " + strokes;
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
}
