using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxPower = 50f; // Increased from 30f for much greater range
    [SerializeField] private float powerBarSpeed = 25f; // Increased from 15f for even faster cursor
    [SerializeField] private float minVelocityToShoot = 0.1f;
    [SerializeField] private float rotationSpeed = 60f; // Reduced from 120f for more precise aiming

    [Header("Aim Settings")]
    [SerializeField] private Transform aimArrow;
    [SerializeField] private float aimArrowLength = 3f; // Length of dot trail
    [SerializeField] private int dotCount = 7; // Number of dots in trail

    private Rigidbody2D rb;
    private float aimAngle = 0f; // Angle in degrees
    private float currentPower = 0f;
    private bool isCharging = false;
    private bool canShoot = true;
    private int ballsRemaining = 5; // Changed from strokeCount
    private PowerBarUI powerBarUI;
    private bool powerBarGoingUp = true;
    private Vector3 startPosition;

    public int BallsRemaining => ballsRemaining;
    public bool IsMoving => rb.linearVelocity.magnitude > minVelocityToShoot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Setup Rigidbody2D
        rb.linearDamping = 2f;
        rb.gravityScale = 0f;
        
        // Create aim arrow if not assigned
        if (aimArrow == null)
        {
            GameObject arrowObj = new GameObject("Aim Arrow");
            arrowObj.transform.SetParent(transform);
            arrowObj.transform.localPosition = Vector3.zero;
            aimArrow = arrowObj.transform;
            
            // Create stream of diminishing dots instead of arrow
            for (int i = 0; i < dotCount; i++)
            {
                GameObject dot = new GameObject($"Dot {i}");
                dot.transform.SetParent(arrowObj.transform);
                
                // Position dots along the aim line
                float t = (i + 1) / (float)dotCount;
                float distance = aimArrowLength * t;
                dot.transform.localPosition = Vector3.right * distance;
                
                // Create dot sprite with diminishing size
                float sizeMultiplier = 1f - (t * 0.7f); // Size reduces from 100% to 30%
                float dotSize = 0.25f * sizeMultiplier;
                dot.transform.localScale = Vector3.one * dotSize;
                
                SpriteRenderer dotSprite = dot.AddComponent<SpriteRenderer>();
                dotSprite.sprite = CreateCircleSprite(Color.white);
                dotSprite.sortingOrder = 10;
                
                // Optional: fade alpha as well for better effect
                Color dotColor = Color.white;
                dotColor.a = 0.7f + (0.3f * (1f - t)); // Fade from opaque to slightly transparent
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
        
        // Draw a right-pointing triangle (arrow head)
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                float normalizedY = (y - 16) / 16f; // -1 to 1
                float normalizedX = x / 32f; // 0 to 1
                
                // Triangle pointing right
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
        powerBarUI = FindObjectOfType<PowerBarUI>();
        startPosition = transform.position; // Remember start position for respawn
        
        // Initialize UI
        GameUI gameUI = FindObjectOfType<GameUI>();
        if (gameUI != null)
        {
            gameUI.UpdateBallsRemaining(ballsRemaining);
        }
    }

    private void Update()
    {
        // Check if ball is nearly stopped
        canShoot = !IsMoving;

        // ALWAYS hide aim arrow when ball is moving OR when charging
        // This prevents the weird thing showing in walls
        if (!canShoot || isCharging)
        {
            if (aimArrow != null && aimArrow.gameObject != null)
            {
                aimArrow.gameObject.SetActive(false);
            }
                
            if (!canShoot)
                return;
        }

        // Only show aim dots when ball is completely stopped and not charging
        if (canShoot && !isCharging)
        {
            if (aimArrow != null && aimArrow.gameObject != null)
            {
                aimArrow.gameObject.SetActive(true);
            }
        }

        // Handle aim rotation input (A/D for 360-degree aiming)
        HandleAimInput();

        // Handle charging and shooting
        HandleShootInput();

        // Update aim arrow rotation
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

        // Keep angle in 0-360 range
        aimAngle = aimAngle % 360f;
    }

    private void HandleShootInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isCharging)
            {
                // Start charging
                isCharging = true;
                currentPower = 0f;
                powerBarGoingUp = true;
                if (powerBarUI != null)
                    powerBarUI.Show();
            }
            else
            {
                // Fire!
                Shoot();
            }
        }

        // Animate power bar while charging
        if (isCharging)
        {
            // Ping-pong power between 0 and maxPower
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

            // Update power bar UI
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

        // Check if player has balls remaining
        if (ballsRemaining <= 0)
        {
            GameUI gameUI = FindObjectOfType<GameUI>();
            if (gameUI != null)
            {
                gameUI.ShowGameOver();
            }
            return;
        }

        // Convert angle to direction vector
        float angleRad = aimAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        // Apply force
        rb.AddForce(direction * currentPower, ForceMode2D.Impulse);

        // Use one ball
        ballsRemaining--;

        // Reset charging
        isCharging = false;
        currentPower = 0f;

        // Hide power bar
        if (powerBarUI != null)
            powerBarUI.Hide();

        // Update UI
        GameUI gameUI2 = FindObjectOfType<GameUI>();
        if (gameUI2 != null)
        {
            gameUI2.UpdateBallsRemaining(ballsRemaining);
        }
    }

    private void UpdateAimArrow()
    {
        if (aimArrow == null || !canShoot || isCharging)
            return;

        // Rotate dots to match aim angle
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
}
