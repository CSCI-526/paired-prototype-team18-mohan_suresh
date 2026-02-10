using UnityEngine;

public class LethalWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        BallController ball = collision.gameObject.GetComponent<BallController>();
        if (ball != null)
        {
            // Ball touched lethal wall - game over immediately
            ball.StopBall();
            
            GameUI gameUI = FindObjectOfType<GameUI>();
            if (gameUI != null)
            {
                gameUI.ShowGameOver();
            }
        }
    }
}
