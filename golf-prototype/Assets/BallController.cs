using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxPower = 50f;
    [SerializeField] private float powerBarSpeed = 25f;
    [SerializeField] private float minVelocityToShoot = 0.1f;
    [SerializeField] private float rotationSpeed = 60f;

    [Header("Aim Settings")]
    [SerializeField] private Transform aimArrow;
    [SerializeField] private float aimArrowLength = 3f;
    [SerializeField] private int dotCount = 7;

    private Rigidbody2D rb;
    private float aimAngle = 0f;
    private float currentPower = 0f;
    private bool isCharging = false;
    private bool canShoot = true;
    private int ballsRemaining = 15;
    private PowerBarUI powerBarUI;
    private GameUI gameUI;
    private bool powerBarGoingUp = true;
    private Vector3 startPosition;

    public int BallsRemaining => ballsRemaining;
    public bool IsMoving => rb.linearVelocity.magnitude > minVelocityToShoot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.linearDamping = 2f;
        rb.gravityScale = 0f;
        
        if (aimArrow == null)
        {
            GameObject arrowObj = new GameObject("Aim Arrow");
            arrowObj.transform.SetParent(transform);
            arrowObj.transform.localPosition = Vector3.zero;
            aimArrow = arrowObj.transform;
            
            for (int i = 0; i < dotCount; i++)
            {
                GameObject dot = new GameObject($"Dot {i}");
                dot.transform.SetParent(arrowObj.transform);
                
                float t = (i + 1) / (float)dotCount;
                float distance = aimArrowLength * t;
                dot.transform.localPosition = Vector3.right * distance;
                
                float sizeMultiplier = 1f - (t * 0.7f);
                float dotSize = 0.25f * sizeMultiplier;
                dot.transform.localScale = Vector3.one * dotSize;
                
                SpriteRenderer dotSprite = dot.AddComponent<SpriteRenderer>();
                dotSprite.sprite = CreateCircleSprite(Color.white);
                dotSprite.sortingOrder = 10;
                
                Color dotColor = Color.white;
                dotColor.a = 0.7f + (0.3f * (1f - t));
                dotSprite.color = dotColor;
            }
        }
    }
    
    private Sprite CreateCircleSprite(Color color)
    {
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                float dx = x - 16;
                float dy = y - 16;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                
                if (distance <= 15)
                {
                    pixels[y * 32 + x] = color;
                }
                else
                {
                    pixels[y * 32 + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }
    
    private Sprite CreateTriangleSprite(Color color)
    {
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                float normalizedY = (y - 16) / 16f;
                float normalizedX = x / 32f;
                
                if (normalizedX >= 0 && Mathf.Abs(normalizedY) <= (1f - normalizedX))
                {
                    pixels[y * 32 + x] = color;
                }
                else
                {
                    pixels[y * 32 + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0, 0.5f), 32);
    }

    private void Start()
    {
        powerBarUI = FindFirstObjectByType<PowerBarUI>();
        gameUI = FindFirstObjectByType<GameUI>();
        startPosition = transform.position;
        
        if (gameUI != null)
        {
            gameUI.UpdateBallsRemaining(ballsRemaining);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestartGame();
            return;
        }
        
        if (gameUI != null && gameUI.IsGameOver)
        {
            if (aimArrow != null && aimArrow.gameObject != null)
                aimArrow.gameObject.SetActive(false);
            return;
        }
        
        canShoot = !IsMoving;

        if (!canShoot || isCharging)
        {
            if (aimArrow != null && aimArrow.gameObject != null)
            {
                aimArrow.gameObject.SetActive(false);
            }
                
            if (!canShoot)
                return;
        }

        if (canShoot && !isCharging)
        {
            if (aimArrow != null && aimArrow.gameObject != null)
            {
                aimArrow.gameObject.SetActive(true);
            }
        }

        HandleAimInput();

        HandleShootInput();

        UpdateAimArrow();
    }

    private void HandleAimInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            aimAngle += rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            aimAngle -= rotationSpeed * Time.deltaTime;
        }

        aimAngle = aimAngle % 360f;
    }

    private void HandleShootInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isCharging)
            {
                isCharging = true;
                currentPower = 0f;
                powerBarGoingUp = true;
                if (powerBarUI != null)
                    powerBarUI.Show();
            }
            else
            {
                Shoot();
            }
        }

        if (isCharging)
        {
            if (powerBarGoingUp)
            {
                currentPower += powerBarSpeed * Time.deltaTime;
                if (currentPower >= maxPower)
                {
                    currentPower = maxPower;
                    powerBarGoingUp = false;
                }
            }
            else
            {
                currentPower -= powerBarSpeed * Time.deltaTime;
                if (currentPower <= 0f)
                {
                    currentPower = 0f;
                    powerBarGoingUp = true;
                }
            }

            if (powerBarUI != null)
            {
                powerBarUI.SetPower(currentPower / maxPower);
            }
        }
    }

    private void Shoot()
    {
        if (!canShoot || !isCharging)
            return;

        if (ballsRemaining <= 0)
        {
            if (gameUI != null)
            {
                gameUI.ShowGameOver();
            }
            return;
        }

        float angleRad = aimAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        rb.AddForce(direction * currentPower, ForceMode2D.Impulse);

        ballsRemaining--;

        isCharging = false;
        currentPower = 0f;

        if (powerBarUI != null)
            powerBarUI.Hide();

        if (gameUI != null)
        {
            gameUI.UpdateBallsRemaining(ballsRemaining);
            
            if (ballsRemaining <= 0)
            {
                gameUI.ShowGameOver();
            }
        }
    }

    private void UpdateAimArrow()
    {
        if (aimArrow == null || !canShoot || isCharging)
            return;

        aimArrow.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    public void StopBall()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        isCharging = false;
        if (powerBarUI != null)
            powerBarUI.Hide();
        if (aimArrow != null && aimArrow.gameObject != null)
            aimArrow.gameObject.SetActive(false);
    }
    
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
