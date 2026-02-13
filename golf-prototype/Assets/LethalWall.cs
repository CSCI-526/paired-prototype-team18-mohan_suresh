using UnityEngine;

public class LethalWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        BallController ball = collision.gameObject.GetComponent<BallController>();
        if (ball != null)
        {
            ball.StopBall();
            
            GameUI gameUI = FindFirstObjectByType<GameUI>();
            if (gameUI != null)
            {
                gameUI.ShowGameOver("GAME OVER!\nYou hit a lethal spiked wall!");
            }
        }
    }
}
