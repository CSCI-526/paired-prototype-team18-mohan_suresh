using UnityEngine;
using System.Collections;

public class HoleController : MonoBehaviour
{
    [Header("Hole Settings")]
    [SerializeField] private BallSize requiredSize = BallSize.Normal;
    [SerializeField] private float rejectForce = 5f;
    [SerializeField] private float sinkDuration = 1f;
    [SerializeField] private float sinkRotationSpeed = 360f;

    private bool ballIsSinking = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ballIsSinking)
            return;

        BallSizeController ballSize = collision.GetComponent<BallSizeController>();
        if (ballSize == null)
            return;

        BallController ballController = collision.GetComponent<BallController>();
        if (ballController == null)
            return;

        // Check if size matches
        if (ballSize.CurrentSize == requiredSize)
        {
            // Win! Start sinking animation
            ballIsSinking = true;
            StartCoroutine(SinkBallAnimation(collision.gameObject, ballController));
        }
        else
        {
            // Reject the ball
            Vector2 rejectDirection = (collision.transform.position - transform.position).normalized;
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(rejectDirection * rejectForce, ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator SinkBallAnimation(GameObject ball, BallController ballController)
    {
        // Stop the ball
        ballController.StopBall();
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false; // Disable physics
        }

        Vector3 startPos = ball.transform.position;
        Vector3 targetPos = transform.position; // Hole center
        Vector3 startScale = ball.transform.localScale;
        Vector3 targetScale = ball.transform.localScale * 0.1f; // Shrink to 10%

        float elapsed = 0f;

        while (elapsed < sinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / sinkDuration;
            
            // Ease-in curve for more realistic sinking
            float smoothT = 1f - Mathf.Pow(1f - t, 3f);

            // Move toward hole center and sink down
            ball.transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            
            // Shrink the ball
            ball.transform.localScale = Vector3.Lerp(startScale, targetScale, smoothT);
            
            // Rotate the ball for effect
            ball.transform.Rotate(0, 0, sinkRotationSpeed * Time.deltaTime);

            yield return null;
        }

        // Hide the ball
        ball.SetActive(false);

        // Show win screen
        GameUI gameUI = FindFirstObjectByType<GameUI>();
        if (gameUI != null)
        {
            gameUI.ShowWin();
        }
    }

    public void SetRequiredSize(BallSize size)
    {
        requiredSize = size;
    }
}
