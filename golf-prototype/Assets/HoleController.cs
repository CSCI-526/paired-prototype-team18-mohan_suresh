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

        if (ballSize.CurrentSize == requiredSize)
        {
            ballIsSinking = true;
            StartCoroutine(SinkBallAnimation(collision.gameObject, ballController));
        }
        else
        {
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
        ballController.StopBall();
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        Vector3 startPos = ball.transform.position;
        Vector3 targetPos = transform.position;
        Vector3 startScale = ball.transform.localScale;
        Vector3 targetScale = ball.transform.localScale * 0.1f;

        float elapsed = 0f;

        while (elapsed < sinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / sinkDuration;
            
            float smoothT = 1f - Mathf.Pow(1f - t, 3f);

            ball.transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            
            ball.transform.localScale = Vector3.Lerp(startScale, targetScale, smoothT);
            
            ball.transform.Rotate(0, 0, sinkRotationSpeed * Time.deltaTime);

            yield return null;
        }

        ball.SetActive(false);

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
