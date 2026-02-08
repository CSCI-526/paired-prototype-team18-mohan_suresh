using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class CourseBuilderEditor
{
    [MenuItem("Tools/Build Golf Prototype Scene")]
    public static void BuildGolfPrototypeScene()
    {
        try
        {
            Debug.Log("Starting to build golf prototype scene...");
            
            // Create Main Camera
            Debug.Log("Creating camera...");
            GameObject cameraObj = GameObject.Find("Main Camera");
            if (cameraObj == null)
            {
                cameraObj = new GameObject("Main Camera");
                cameraObj.tag = "MainCamera";
            }
            
            Camera camera = cameraObj.GetComponent<Camera>();
            if (camera == null)
                camera = cameraObj.AddComponent<Camera>();
                
            camera.orthographic = true;
            camera.orthographicSize = 10f;
            camera.transform.position = new Vector3(0, 0, -10);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.1f, 0.5f, 0.1f);

            // Create Canvas
            Debug.Log("Creating canvas...");
            GameObject canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // Create UI Panel for main UI
            Debug.Log("Creating UI panel...");
            GameObject uiPanel = new GameObject("UI Panel");
            uiPanel.transform.SetParent(canvasObj.transform, false);
            RectTransform uiRect = uiPanel.AddComponent<RectTransform>();
            uiRect.anchorMin = new Vector2(0, 1);
            uiRect.anchorMax = new Vector2(0, 1);
            uiRect.pivot = new Vector2(0, 1);
            uiRect.anchoredPosition = new Vector2(20, -20);
            uiRect.sizeDelta = new Vector2(300, 100);

            // Create Strokes Text
            Debug.Log("Creating strokes text...");
            GameObject strokesTextObj = new GameObject("Strokes Text");
            strokesTextObj.transform.SetParent(uiPanel.transform, false);
            RectTransform strokesRect = strokesTextObj.AddComponent<RectTransform>();
            strokesRect.anchorMin = new Vector2(0, 1);
            strokesRect.anchorMax = new Vector2(1, 1);
            strokesRect.pivot = new Vector2(0, 1);
            strokesRect.anchoredPosition = new Vector2(0, 0);
            strokesRect.sizeDelta = new Vector2(0, 30);
            Text strokesText = strokesTextObj.AddComponent<Text>();
            strokesText.text = "Strokes: 0";
            strokesText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            strokesText.fontSize = 20;
            strokesText.color = Color.white;

            // Create Ball Size Text
            Debug.Log("Creating ball size text...");
            GameObject sizeTextObj = new GameObject("Ball Size Text");
            sizeTextObj.transform.SetParent(uiPanel.transform, false);
            RectTransform sizeRect = sizeTextObj.AddComponent<RectTransform>();
            sizeRect.anchorMin = new Vector2(0, 1);
            sizeRect.anchorMax = new Vector2(1, 1);
            sizeRect.pivot = new Vector2(0, 1);
            sizeRect.anchoredPosition = new Vector2(0, -35);
            sizeRect.sizeDelta = new Vector2(0, 30);
            Text sizeText = sizeTextObj.AddComponent<Text>();
            sizeText.text = "Ball Size: Normal";
            sizeText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            sizeText.fontSize = 20;
            sizeText.color = Color.white;

            // Create Instructions Text
            Debug.Log("Creating instructions text...");
            GameObject instructionsTextObj = new GameObject("Instructions Text");
            instructionsTextObj.transform.SetParent(canvasObj.transform, false);
            RectTransform instructionsRect = instructionsTextObj.AddComponent<RectTransform>();
            instructionsRect.anchorMin = new Vector2(0.5f, 0);
            instructionsRect.anchorMax = new Vector2(0.5f, 0);
            instructionsRect.pivot = new Vector2(0.5f, 0);
            instructionsRect.anchoredPosition = new Vector2(0, 20);
            instructionsRect.sizeDelta = new Vector2(600, 30);
            Text instructionsText = instructionsTextObj.AddComponent<Text>();
            instructionsText.text = "A/D: Rotate Aim | SPACE: Charge Power (press again to shoot)";
            instructionsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            instructionsText.fontSize = 16;
            instructionsText.color = Color.yellow;
            instructionsText.alignment = TextAnchor.MiddleCenter;

            // Create Win Panel
            Debug.Log("Creating win panel...");
            GameObject winPanel = new GameObject("Win Panel");
            winPanel.transform.SetParent(canvasObj.transform, false);
            RectTransform winRect = winPanel.AddComponent<RectTransform>();
            winRect.anchorMin = Vector2.zero;
            winRect.anchorMax = Vector2.one;
            winRect.sizeDelta = Vector2.zero;
            Image winImage = winPanel.AddComponent<Image>();
            winImage.color = new Color(0, 0, 0, 0.8f);

            GameObject winTextObj = new GameObject("Win Text");
            winTextObj.transform.SetParent(winPanel.transform, false);
            RectTransform winTextRect = winTextObj.AddComponent<RectTransform>();
            winTextRect.anchorMin = Vector2.zero;
            winTextRect.anchorMax = Vector2.one;
            winTextRect.sizeDelta = Vector2.zero;
            Text winText = winTextObj.AddComponent<Text>();
            winText.text = "YOU WIN!";
            winText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            winText.fontSize = 60;
            winText.color = Color.green;
            winText.alignment = TextAnchor.MiddleCenter;
            winText.fontStyle = FontStyle.Bold;

            winPanel.SetActive(false);

            // Create Power Bar UI
            Debug.Log("Creating power bar...");
            GameObject powerBarPanel = new GameObject("Power Bar Panel");
            powerBarPanel.transform.SetParent(canvasObj.transform, false);
            RectTransform powerBarRect = powerBarPanel.AddComponent<RectTransform>();
            powerBarRect.anchorMin = new Vector2(1, 0.5f);
            powerBarRect.anchorMax = new Vector2(1, 0.5f);
            powerBarRect.pivot = new Vector2(1, 0.5f);
            powerBarRect.anchoredPosition = new Vector2(-30, 0);
            powerBarRect.sizeDelta = new Vector2(60, 450);
            
            // Background
            Image powerBarBg = powerBarPanel.AddComponent<Image>();
            powerBarBg.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            
            // Create 3 color zones (Low, Medium, High)
            // Low power zone (Green) - bottom third
            GameObject lowZone = new GameObject("Low Zone");
            lowZone.transform.SetParent(powerBarPanel.transform, false);
            RectTransform lowRect = lowZone.AddComponent<RectTransform>();
            lowRect.anchorMin = new Vector2(0.1f, 0.05f);
            lowRect.anchorMax = new Vector2(0.9f, 0.35f);
            lowRect.offsetMin = Vector2.zero;
            lowRect.offsetMax = Vector2.zero;
            Image lowImage = lowZone.AddComponent<Image>();
            lowImage.color = Color.green;
            
            // Medium power zone (Orange) - middle third
            GameObject medZone = new GameObject("Medium Zone");
            medZone.transform.SetParent(powerBarPanel.transform, false);
            RectTransform medRect = medZone.AddComponent<RectTransform>();
            medRect.anchorMin = new Vector2(0.1f, 0.37f);
            medRect.anchorMax = new Vector2(0.9f, 0.63f);
            medRect.offsetMin = Vector2.zero;
            medRect.offsetMax = Vector2.zero;
            Image medImage = medZone.AddComponent<Image>();
            medImage.color = new Color(1f, 0.5f, 0f); // Orange
            
            // High power zone (Red) - top third
            GameObject highZone = new GameObject("High Zone");
            highZone.transform.SetParent(powerBarPanel.transform, false);
            RectTransform highRect = highZone.AddComponent<RectTransform>();
            highRect.anchorMin = new Vector2(0.1f, 0.65f);
            highRect.anchorMax = new Vector2(0.9f, 0.95f);
            highRect.offsetMin = Vector2.zero;
            highRect.offsetMax = Vector2.zero;
            Image highImage = highZone.AddComponent<Image>();
            highImage.color = Color.red;
            
            // Create pointer (triangle/arrow)
            GameObject pointer = new GameObject("Pointer");
            pointer.transform.SetParent(powerBarPanel.transform, false);
            RectTransform pointerRect = pointer.AddComponent<RectTransform>();
            pointerRect.anchorMin = new Vector2(0.5f, 0.5f);
            pointerRect.anchorMax = new Vector2(0.5f, 0.5f);
            pointerRect.pivot = new Vector2(0, 0.5f);
            pointerRect.anchoredPosition = Vector2.zero;
            pointerRect.sizeDelta = new Vector2(40, 15);
            Image pointerImage = pointer.AddComponent<Image>();
            pointerImage.sprite = CreateTriangleSprite(Color.white);
            pointerImage.color = Color.white;
            
            // Labels
            GameObject maxLabel = new GameObject("Max Label");
            maxLabel.transform.SetParent(powerBarPanel.transform, false);
            RectTransform maxLabelRect = maxLabel.AddComponent<RectTransform>();
            maxLabelRect.anchorMin = new Vector2(0.5f, 1);
            maxLabelRect.anchorMax = new Vector2(0.5f, 1);
            maxLabelRect.pivot = new Vector2(0.5f, 0);
            maxLabelRect.anchoredPosition = new Vector2(0, 5);
            maxLabelRect.sizeDelta = new Vector2(60, 20);
            Text maxText = maxLabel.AddComponent<Text>();
            maxText.text = "MAX";
            maxText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            maxText.fontSize = 14;
            maxText.color = Color.white;
            maxText.alignment = TextAnchor.MiddleCenter;
            maxText.fontStyle = FontStyle.Bold;
            
            GameObject minLabel = new GameObject("Min Label");
            minLabel.transform.SetParent(powerBarPanel.transform, false);
            RectTransform minLabelRect = minLabel.AddComponent<RectTransform>();
            minLabelRect.anchorMin = new Vector2(0.5f, 0);
            minLabelRect.anchorMax = new Vector2(0.5f, 0);
            minLabelRect.pivot = new Vector2(0.5f, 1);
            minLabelRect.anchoredPosition = new Vector2(0, -5);
            minLabelRect.sizeDelta = new Vector2(60, 20);
            Text minText = minLabel.AddComponent<Text>();
            minText.text = "MIN";
            minText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            minText.fontSize = 14;
            minText.color = Color.white;
            minText.alignment = TextAnchor.MiddleCenter;
            minText.fontStyle = FontStyle.Bold;
            
            powerBarPanel.SetActive(false);
            
            // Add PowerBarUI component
            GameObject powerBarUIObj = new GameObject("Power Bar Controller");
            powerBarUIObj.transform.SetParent(canvasObj.transform);
            PowerBarUI powerBarUI = powerBarUIObj.AddComponent<PowerBarUI>();
            SerializedObject serializedPowerBar = new SerializedObject(powerBarUI);
            serializedPowerBar.FindProperty("pointer").objectReferenceValue = pointerRect;
            serializedPowerBar.FindProperty("powerBarPanel").objectReferenceValue = powerBarPanel;
            serializedPowerBar.FindProperty("barRect").objectReferenceValue = powerBarRect;
            serializedPowerBar.ApplyModifiedProperties();

            // Create GameUI component
            Debug.Log("Creating GameUI...");
            GameObject gameUIObj = new GameObject("Game Manager");
            GameUI gameUI = gameUIObj.AddComponent<GameUI>();
            
            SerializedObject serializedGameUI = new SerializedObject(gameUI);
            serializedGameUI.FindProperty("strokesText").objectReferenceValue = strokesText;
            serializedGameUI.FindProperty("ballSizeText").objectReferenceValue = sizeText;
            serializedGameUI.FindProperty("instructionsText").objectReferenceValue = instructionsText;
            serializedGameUI.FindProperty("winPanel").objectReferenceValue = winPanel;
            serializedGameUI.ApplyModifiedProperties();

            // Create Ball
            Debug.Log("Creating ball...");
            GameObject ball = new GameObject("Ball");
            ball.transform.position = new Vector3(-8, -8, 0);
            
            // Add sprite renderer with circle sprite
            SpriteRenderer ballSprite = ball.AddComponent<SpriteRenderer>();
            ballSprite.sprite = CreateCircleSprite(Color.white);
            ballSprite.sortingOrder = 1;
            
            Rigidbody2D ballRb = ball.AddComponent<Rigidbody2D>();
            ballRb.gravityScale = 0f;
            ballRb.linearDamping = 2f;
            ballRb.angularDamping = 1f;
            ballRb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            
            // Add physics material for bouncing
            PhysicsMaterial2D ballMaterial = new PhysicsMaterial2D("Ball Material");
            ballMaterial.bounciness = 0.6f; // 60% bounce
            ballMaterial.friction = 0.3f;
            
            CircleCollider2D ballCollider = ball.AddComponent<CircleCollider2D>();
            ballCollider.radius = 0.5f;
            ballCollider.sharedMaterial = ballMaterial;
            
            BallController ballController = ball.AddComponent<BallController>();
            BallSizeController ballSizeController = ball.AddComponent<BallSizeController>();

            // Create boundary walls
            Debug.Log("Creating walls...");
            CreateWall("Top Wall", new Vector3(0, 10, 0), new Vector2(22, 1), Color.gray);
            CreateWall("Bottom Wall", new Vector3(0, -10, 0), new Vector2(22, 1), Color.gray);
            CreateWall("Left Wall", new Vector3(-10, 0, 0), new Vector2(1, 22), Color.gray);
            CreateWall("Right Wall", new Vector3(10, 0, 0), new Vector2(1, 22), Color.gray);

            // Create inner obstacle walls
            CreateWall("Inner Wall 1", new Vector3(-5, 3, 0), new Vector2(1, 8), Color.gray);
            CreateWall("Inner Wall 2", new Vector3(5, -3, 0), new Vector2(1, 8), Color.gray);
            CreateWall("Inner Wall 3", new Vector3(0, 0, 0), new Vector2(10, 1), Color.gray);

            // Create Red (Shrink) zones
            Debug.Log("Creating size zones...");
            CreateSizeZone("Red Zone 1", new Vector3(-5, 8, 0), new Vector2(3, 1), Color.red, SizeZoneType.Shrink);
            CreateSizeZone("Red Zone 2", new Vector3(0, -4, 0), new Vector2(4, 1), Color.red, SizeZoneType.Shrink);

            // Create Blue (Grow) zones
            CreateSizeZone("Blue Zone 1", new Vector3(5, 8, 0), new Vector2(3, 1), Color.blue, SizeZoneType.Grow);
            CreateSizeZone("Blue Zone 2", new Vector3(-5, -4, 0), new Vector2(4, 1), Color.blue, SizeZoneType.Grow);

            // Create Final Hole
            Debug.Log("Creating hole...");
            GameObject hole = new GameObject("Final Hole");
            hole.transform.position = new Vector3(8, 8, 0);
            
            SpriteRenderer holeSprite = hole.AddComponent<SpriteRenderer>();
            holeSprite.sprite = CreateCircleSprite(new Color(0.2f, 0.8f, 0.2f));
            hole.transform.localScale = Vector3.one * 1.5f;
            
            CircleCollider2D holeCollider = hole.AddComponent<CircleCollider2D>();
            holeCollider.radius = 0.5f;
            holeCollider.isTrigger = true;
            
            HoleController holeController = hole.AddComponent<HoleController>();
            
            SerializedObject serializedHole = new SerializedObject(holeController);
            serializedHole.FindProperty("requiredSize").enumValueIndex = (int)BallSize.Normal;
            serializedHole.ApplyModifiedProperties();
            
            // Create Flag above hole
            Debug.Log("Creating flag...");
            CreateFlag(new Vector3(8, 8, 0));

            // Mark scene as dirty
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Debug.Log("Golf Prototype Scene built successfully!");
            EditorUtility.DisplayDialog("Success", "Golf Prototype Scene built successfully!\n\nPress Play to test the game.", "OK");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error building scene: " + e.Message);
            Debug.LogError("Stack trace: " + e.StackTrace);
            EditorUtility.DisplayDialog("Error", "Failed to build scene. Check console for details.", "OK");
        }
    }

    private static void CreateWall(string name, Vector3 position, Vector2 size, Color color)
    {
        GameObject wall = new GameObject(name);
        wall.transform.position = position;
        wall.transform.localScale = new Vector3(size.x, size.y, 1);
        
        SpriteRenderer sprite = wall.AddComponent<SpriteRenderer>();
        sprite.color = color;
        sprite.sprite = CreateSquareSprite(color);
        
        // Add physics material for bouncing
        PhysicsMaterial2D wallMaterial = new PhysicsMaterial2D("Wall Material");
        wallMaterial.bounciness = 0.8f; // 80% bounce for walls
        wallMaterial.friction = 0.1f;
        
        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = Vector2.one;
        collider.sharedMaterial = wallMaterial;
    }

    private static void CreateSizeZone(string name, Vector3 position, Vector2 size, Color color, SizeZoneType zoneType)
    {
        GameObject zone = new GameObject(name);
        zone.transform.position = position;
        zone.transform.localScale = new Vector3(size.x, size.y, 1);
        
        SpriteRenderer sprite = zone.AddComponent<SpriteRenderer>();
        sprite.color = color;
        sprite.sprite = CreateSquareSprite(color);
        
        // Add physics material for bouncing
        PhysicsMaterial2D zoneMaterial = new PhysicsMaterial2D("Zone Material");
        zoneMaterial.bounciness = 0.8f;
        zoneMaterial.friction = 0.1f;
        
        BoxCollider2D collider = zone.AddComponent<BoxCollider2D>();
        collider.size = Vector2.one;
        collider.sharedMaterial = zoneMaterial;
        
        SizeZone sizeZone = zone.AddComponent<SizeZone>();
        sizeZone.SetZoneType(zoneType);
    }
    
    private static void CreateFlag(Vector3 holePosition)
    {
        GameObject flag = new GameObject("Flag");
        flag.transform.position = holePosition;
        
        // Create pole
        GameObject pole = new GameObject("Pole");
        pole.transform.SetParent(flag.transform);
        pole.transform.localPosition = new Vector3(0, 1.5f, 0);
        pole.transform.localScale = new Vector3(0.08f, 3f, 1f);
        
        SpriteRenderer poleSprite = pole.AddComponent<SpriteRenderer>();
        poleSprite.sprite = CreateSquareSprite(Color.yellow);
        poleSprite.sortingOrder = 2;
        
        // Create flag cloth (triangle)
        GameObject cloth = new GameObject("Flag Cloth");
        cloth.transform.SetParent(flag.transform);
        cloth.transform.localPosition = new Vector3(0.4f, 2.5f, 0);
        cloth.transform.localScale = new Vector3(0.8f, 0.6f, 1f);
        
        SpriteRenderer clothSprite = cloth.AddComponent<SpriteRenderer>();
        clothSprite.sprite = CreateTriangleSprite(Color.red);
        clothSprite.sortingOrder = 3;
    }

    private static Sprite CreateCircleSprite(Color color)
    {
        Texture2D texture = new Texture2D(64, 64);
        Color[] pixels = new Color[64 * 64];
        
        for (int y = 0; y < 64; y++)
        {
            for (int x = 0; x < 64; x++)
            {
                float dx = x - 32;
                float dy = y - 32;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                
                if (distance <= 30)
                {
                    pixels[y * 64 + x] = color;
                }
                else
                {
                    pixels[y * 64 + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 64);
    }

    private static Sprite CreateSquareSprite(Color color)
    {
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }
    
    private static Sprite CreateTriangleSprite(Color color)
    {
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        
        // Draw a right-pointing triangle
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                // Triangle: points at (0, middle), (width, top), (width, bottom)
                // Check if point is inside triangle
                float normalizedY = y / 32f;
                float triangleWidth = normalizedY <= 0.5f ? (normalizedY * 2f) : ((1f - normalizedY) * 2f);
                
                if (x <= triangleWidth * 32)
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
}
