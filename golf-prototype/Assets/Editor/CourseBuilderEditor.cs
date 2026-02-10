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

            // Create Balls Remaining Text
            Debug.Log("Creating balls remaining text...");
            GameObject ballsTextObj = new GameObject("Balls Remaining Text");
            ballsTextObj.transform.SetParent(uiPanel.transform, false);
            RectTransform ballsRect = ballsTextObj.AddComponent<RectTransform>();
            ballsRect.anchorMin = new Vector2(0, 1);
            ballsRect.anchorMax = new Vector2(1, 1);
            ballsRect.pivot = new Vector2(0, 1);
            ballsRect.anchoredPosition = new Vector2(0, 0);
            ballsRect.sizeDelta = new Vector2(0, 30);
            Text ballsText = ballsTextObj.AddComponent<Text>();
            ballsText.text = "Balls: 5/5";
            ballsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            ballsText.fontSize = 20;
            ballsText.color = Color.white;

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
            instructionsText.text = "A/D: Rotate Aim | SPACE: Charge Power (press again to shoot) | ESC: Restart";
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
            
            // Create Game Over Panel
            Debug.Log("Creating game over panel...");
            GameObject gameOverPanel = new GameObject("Game Over Panel");
            gameOverPanel.transform.SetParent(canvasObj.transform, false);
            RectTransform gameOverRect = gameOverPanel.AddComponent<RectTransform>();
            gameOverRect.anchorMin = Vector2.zero;
            gameOverRect.anchorMax = Vector2.one;
            gameOverRect.sizeDelta = Vector2.zero;
            Image gameOverImage = gameOverPanel.AddComponent<Image>();
            gameOverImage.color = new Color(0, 0, 0, 0.8f);

            GameObject gameOverTextObj = new GameObject("Game Over Text");
            gameOverTextObj.transform.SetParent(gameOverPanel.transform, false);
            RectTransform gameOverTextRect = gameOverTextObj.AddComponent<RectTransform>();
            gameOverTextRect.anchorMin = Vector2.zero;
            gameOverTextRect.anchorMax = Vector2.one;
            gameOverTextRect.sizeDelta = Vector2.zero;
            Text gameOverText = gameOverTextObj.AddComponent<Text>();
            gameOverText.text = "GAME OVER!\nNo Balls Remaining";
            gameOverText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            gameOverText.fontSize = 50;
            gameOverText.color = Color.red;
            gameOverText.alignment = TextAnchor.MiddleCenter;
            gameOverText.fontStyle = FontStyle.Bold;

            gameOverPanel.SetActive(false);

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
            serializedGameUI.FindProperty("ballsRemainingText").objectReferenceValue = ballsText;
            serializedGameUI.FindProperty("ballSizeText").objectReferenceValue = sizeText;
            serializedGameUI.FindProperty("instructionsText").objectReferenceValue = instructionsText;
            serializedGameUI.FindProperty("winPanel").objectReferenceValue = winPanel;
            serializedGameUI.FindProperty("gameOverPanel").objectReferenceValue = gameOverPanel;
            serializedGameUI.FindProperty("gameOverText").objectReferenceValue = gameOverText;
            serializedGameUI.ApplyModifiedProperties();

            // Create Ball
            Debug.Log("Creating ball...");
            GameObject ball = new GameObject("Ball");
            ball.transform.position = new Vector3(-7.5f, -7f, 0);
            
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

            // EXACT COURSE LAYOUT FROM DESIGN
            // Ball starts bottom-left, must navigate to top-right hole
            
            // Create boundary walls (implicit - not shown but needed)
            Debug.Log("Creating walls...");
            CreateWall("Top Wall", new Vector3(0, 10, 0), new Vector2(22, 0.5f), Color.gray);
            CreateWall("Bottom Wall", new Vector3(0, -10, 0), new Vector2(22, 0.5f), Color.gray);
            CreateWall("Left Wall", new Vector3(-10, 0, 0), new Vector2(0.5f, 22), Color.gray);
            CreateWall("Right Wall", new Vector3(10, 0, 0), new Vector2(0.5f, 22), Color.gray);

            // LEFT-SIDE STRUCTURES
            // Far left bottom: Small grey horizontal bar (bottom-left corner)
            CreateWall("Far Left Grey Base", new Vector3(-8.5f, -8f, 0), new Vector2(3f, 1f), Color.gray);
            
            // Far left: Small Red vertical bar (above grey bar)
            CreateSizeZone("Red Zone Far Left", new Vector3(-9f, -5.5f, 0), new Vector2(1f, 4f), Color.red, SizeZoneType.Shrink);
            
            // Left-center: Grey L-shaped structure
            CreateWall("Left Vertical Wall", new Vector3(-5f, -0.5f, 0), new Vector2(1.5f, 14f), Color.gray);
            CreateWall("Left Horizontal Wall", new Vector3(-7f, 6.5f, 0), new Vector2(4.5f, 1.5f), Color.gray);
            
            // Blue vertical bar right of L-structure
            CreateSizeZone("Blue Zone Left", new Vector3(-3.5f, 0.5f, 0), new Vector2(1.3f, 6f), Color.blue, SizeZoneType.Grow);

            // BOTTOM-LEFT TO BOTTOM-CENTER PATH
            // Diagonal Red Wall - starts mid-left, extends toward center-right
            GameObject diagRedWall = new GameObject("Diagonal Red Wall");
            diagRedWall.transform.position = new Vector3(-0.5f, -3.5f, 0);
            diagRedWall.transform.rotation = Quaternion.Euler(0, 0, 18f);
            diagRedWall.transform.localScale = new Vector3(6f, 1.3f, 1f);
            SpriteRenderer diagRedSprite = diagRedWall.AddComponent<SpriteRenderer>();
            diagRedSprite.sprite = CreateSquareSprite(Color.red);
            BoxCollider2D diagRedCollider = diagRedWall.AddComponent<BoxCollider2D>();
            diagRedCollider.size = Vector2.one;
            PhysicsMaterial2D diagRedMat = new PhysicsMaterial2D(); diagRedMat.bounciness = 0.8f;
            diagRedCollider.sharedMaterial = diagRedMat;
            diagRedWall.AddComponent<SizeZone>().SetZoneType(SizeZoneType.Shrink);

            // CENTER STRUCTURES
            // Tall Vertical Wall (Main Divider) - tall grey wall in center-right area
            CreateWall("Tall Vertical Main Wall", new Vector3(4f, 0.5f, 0), new Vector2(1.5f, 18f), Color.gray);
            
            // Blue Wall Zone - LEFT side of tall wall, lower portion
            CreateSizeZone("Blue Zone Center-Left", new Vector3(2f, -2.5f, 0), new Vector2(1.3f, 5f), Color.blue, SizeZoneType.Grow);
            
            // Red Wall Zone - RIGHT side of tall wall, upper portion
            CreateSizeZone("Red Zone Center-Right", new Vector3(5f, 3.5f, 0), new Vector2(1.3f, 5f), Color.red, SizeZoneType.Shrink);

            // BOTTOM-RIGHT JUNCTION (V-shaped with extensions)
            // Small Red angled piece (upper part of junction)
            GameObject angledRed = new GameObject("Angled Red Wall Junction");
            angledRed.transform.position = new Vector3(5f, -4f, 0);
            angledRed.transform.rotation = Quaternion.Euler(0, 0, 50f);
            angledRed.transform.localScale = new Vector3(2f, 1f, 1f);
            SpriteRenderer angledRedSprite = angledRed.AddComponent<SpriteRenderer>();
            angledRedSprite.sprite = CreateSquareSprite(Color.red);
            BoxCollider2D angledRedCollider = angledRed.AddComponent<BoxCollider2D>();
            angledRedCollider.size = Vector2.one;
            PhysicsMaterial2D angledRedMat = new PhysicsMaterial2D(); angledRedMat.bounciness = 0.8f;
            angledRedCollider.sharedMaterial = angledRedMat;
            angledRed.AddComponent<SizeZone>().SetZoneType(SizeZoneType.Shrink);
            
            // Blue angled wall - middle piece
            GameObject angledBlue1 = new GameObject("Angled Blue Wall Junction");
            angledBlue1.transform.position = new Vector3(4f, -5.5f, 0);
            angledBlue1.transform.rotation = Quaternion.Euler(0, 0, -40f);
            angledBlue1.transform.localScale = new Vector3(3f, 1.2f, 1f);
            SpriteRenderer angledBlue1Sprite = angledBlue1.AddComponent<SpriteRenderer>();
            angledBlue1Sprite.sprite = CreateSquareSprite(Color.blue);
            BoxCollider2D angledBlue1Collider = angledBlue1.AddComponent<BoxCollider2D>();
            angledBlue1Collider.size = Vector2.one;
            PhysicsMaterial2D angledBlue1Mat = new PhysicsMaterial2D(); angledBlue1Mat.bounciness = 0.8f;
            angledBlue1Collider.sharedMaterial = angledBlue1Mat;
            angledBlue1.AddComponent<SizeZone>().SetZoneType(SizeZoneType.Grow);
            
            // Additional Blue angled wall - bottom right extension
            GameObject angledBlue2 = new GameObject("Additional Blue Angled Wall");
            angledBlue2.transform.position = new Vector3(5.5f, -7.5f, 0);
            angledBlue2.transform.rotation = Quaternion.Euler(0, 0, 60f);
            angledBlue2.transform.localScale = new Vector3(2.5f, 1f, 1f);
            SpriteRenderer angledBlue2Sprite = angledBlue2.AddComponent<SpriteRenderer>();
            angledBlue2Sprite.sprite = CreateSquareSprite(Color.blue);
            BoxCollider2D angledBlue2Collider = angledBlue2.AddComponent<BoxCollider2D>();
            angledBlue2Collider.size = Vector2.one;
            PhysicsMaterial2D angledBlue2Mat = new PhysicsMaterial2D(); angledBlue2Mat.bounciness = 0.8f;
            angledBlue2Collider.sharedMaterial = angledBlue2Mat;
            angledBlue2.AddComponent<SizeZone>().SetZoneType(SizeZoneType.Grow);

            // TOP-CENTER OBSTACLE
            // Horizontal Red Wall - near top edge, wide center bar
            CreateSizeZone("Top Center Red Wall", new Vector3(-1f, 8.5f, 0), new Vector2(10f, 1.2f), Color.red, SizeZoneType.Shrink);

            // LETHAL SPIKY WALLS
            // Top-Left Spikes - teeth pointing downward (short section)
            Debug.Log("Creating lethal spiky walls...");
            CreateSpikyLethalWall("Top-Left Spikes", new Vector3(-7.5f, 9.2f, 0), new Vector2(5f, 0.8f), true);
            
            // Right-Edge Spikes - teeth pointing leftward (ONLY lower-middle section, NOT full height)
            CreateSpikyLethalWall("Right-Edge Spikes", new Vector3(9.2f, -2f, 0), new Vector2(0.8f, 12f), false);
            
            // Far Right Blue Zone (near right spikes, lower area)
            CreateSizeZone("Blue Zone Far Right", new Vector3(7.5f, -3f, 0), new Vector2(1.5f, 5f), Color.blue, SizeZoneType.Grow);

            // Create Final Hole - positioned so player must navigate carefully
            Debug.Log("Creating hole...");
            GameObject hole = new GameObject("Final Hole");
            hole.transform.position = new Vector3(8.2f, 6.5f, 0); // Top-right quadrant
            
            SpriteRenderer holeSprite = hole.AddComponent<SpriteRenderer>();
            holeSprite.sprite = CreateOvalSprite(Color.black); // Black oval hole
            hole.transform.localScale = new Vector3(1.8f, 1.2f, 1f); // Oval shape (wider than tall)
            
            CircleCollider2D holeCollider = hole.AddComponent<CircleCollider2D>();
            holeCollider.radius = 0.5f;
            holeCollider.isTrigger = true;
            
            HoleController holeController = hole.AddComponent<HoleController>();
            
            SerializedObject serializedHole = new SerializedObject(holeController);
            serializedHole.FindProperty("requiredSize").enumValueIndex = (int)BallSize.Normal;
            serializedHole.ApplyModifiedProperties();
            
            // Create Flag above hole
            Debug.Log("Creating flag...");
            CreateFlag(new Vector3(8.2f, 6.5f, 0)); // Match hole position

            // Mark scene as dirty
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            // Frame the scene view to show the game area
            if (SceneView.lastActiveSceneView != null)
            {
                SceneView sceneView = SceneView.lastActiveSceneView;
                sceneView.in2DMode = true; // Switch to 2D mode
                sceneView.pivot = Vector3.zero; // Center on origin
                sceneView.size = 15f; // Zoom to show full play area
                sceneView.Repaint();
            }

            Debug.Log("Golf Prototype Scene built successfully!");
            EditorUtility.DisplayDialog("Success", "Golf Prototype Scene built successfully!\n\nPress Play to test the game.\n\nScene view has been set to 2D mode and centered.", "OK");
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
        wall.transform.localScale = new Vector3(size.x, size.y, 1f);
        
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
        zone.transform.localScale = new Vector3(size.x, size.y, 1f);
        
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
        
        // Create pole - WHITE color
        GameObject pole = new GameObject("Pole");
        pole.transform.SetParent(flag.transform);
        pole.transform.localPosition = new Vector3(0, 0.8f, 0); // Lower position
        pole.transform.localScale = new Vector3(0.06f, 1.6f, 1f); // Shorter pole
        
        SpriteRenderer poleSprite = pole.AddComponent<SpriteRenderer>();
        poleSprite.sprite = CreateSquareSprite(Color.white); // WHITE pole
        poleSprite.sortingOrder = 2;
        
        // Create flag cloth (triangle) - RED, attached to pole
        GameObject cloth = new GameObject("Flag Cloth");
        cloth.transform.SetParent(flag.transform);
        cloth.transform.localPosition = new Vector3(0.3f, 1.3f, 0); // Attached to top of pole
        cloth.transform.localScale = new Vector3(0.5f, 0.4f, 1f); // Smaller flag
        
        SpriteRenderer clothSprite = cloth.AddComponent<SpriteRenderer>();
        clothSprite.sprite = CreateTriangleSprite(Color.red); // RED flag
        clothSprite.sortingOrder = 3;
    }
    
    private static void CreateSpikyLethalWall(string name, Vector3 position, Vector2 size, bool horizontal)
    {
        GameObject wallParent = new GameObject(name);
        wallParent.transform.position = position;
        
        // Create sawtooth pattern with base against boundary
        float toothWidth = 0.5f;
        float toothHeight = 0.8f;
        
        int toothCount;
        if (horizontal)
        {
            toothCount = Mathf.CeilToInt(size.x / toothWidth);
        }
        else
        {
            toothCount = Mathf.CeilToInt(size.y / toothWidth);
        }
        
        // Create a single sprite with sawtooth pattern
        GameObject sawtoothObj = new GameObject("Sawtooth");
        sawtoothObj.transform.SetParent(wallParent.transform);
        sawtoothObj.transform.localPosition = Vector3.zero;
        
        SpriteRenderer sawtoothSprite = sawtoothObj.AddComponent<SpriteRenderer>();
        sawtoothSprite.sprite = CreateSawtoothSprite(Color.white, toothCount, horizontal);
        sawtoothSprite.sortingOrder = 2;
        
        if (horizontal)
        {
            // Horizontal sawtooth (teeth pointing down, base at top)
            sawtoothObj.transform.localScale = new Vector3(size.x / toothCount, toothHeight, 1f);
        }
        else
        {
            // Vertical sawtooth (teeth pointing left, base at right)
            sawtoothObj.transform.localScale = new Vector3(toothHeight, size.y / toothCount, 1f);
        }
        
        // Add polygon collider with sawtooth shape
        PolygonCollider2D sawtoothCollider = sawtoothObj.AddComponent<PolygonCollider2D>();
        
        // Create sawtooth polygon points
        Vector2[] points = new Vector2[(toothCount + 1) * 2];
        
        if (horizontal)
        {
            // Horizontal sawtooth
            for (int i = 0; i <= toothCount; i++)
            {
                float x = (i / (float)toothCount) - 0.5f;
                // Top edge (base of triangles)
                points[i * 2] = new Vector2(x, 0.5f);
                // Bottom edge (points of triangles)
                if (i < toothCount)
                {
                    points[i * 2 + 1] = new Vector2(x + (0.5f / toothCount), -0.5f);
                }
            }
            // Close the polygon
            points[points.Length - 1] = points[0];
        }
        else
        {
            // Vertical sawtooth
            for (int i = 0; i <= toothCount; i++)
            {
                float y = (i / (float)toothCount) - 0.5f;
                // Right edge (base of triangles)
                points[i * 2] = new Vector2(0.5f, y);
                // Left edge (points of triangles)
                if (i < toothCount)
                {
                    points[i * 2 + 1] = new Vector2(-0.5f, y + (0.5f / toothCount));
                }
            }
            // Close the polygon
            points[points.Length - 1] = points[0];
        }
        
        sawtoothCollider.SetPath(0, points);
        
        // No bounciness - instant stop
        PhysicsMaterial2D lethalMaterial = new PhysicsMaterial2D("Lethal Material");
        lethalMaterial.bounciness = 0f;
        lethalMaterial.friction = 0f;
        sawtoothCollider.sharedMaterial = lethalMaterial;
        
        // Add LethalWall component - game ends on touch
        sawtoothObj.AddComponent<LethalWall>();
    }
    
    private static Sprite CreateSawtoothSprite(Color color, int toothCount, bool horizontal)
    {
        int texWidth = horizontal ? toothCount * 32 : 32;
        int texHeight = horizontal ? 32 : toothCount * 32;
        
        Texture2D texture = new Texture2D(texWidth, texHeight);
        Color[] pixels = new Color[texWidth * texHeight];
        
        // Initialize to transparent
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }
        
        if (horizontal)
        {
            // Horizontal sawtooth pattern
            for (int y = 0; y < texHeight; y++)
            {
                for (int x = 0; x < texWidth; x++)
                {
                    int toothIndex = x / 32;
                    int xInTooth = x % 32;
                    
                    // Normalized position in tooth (0 to 1)
                    float normX = xInTooth / 32f;
                    float normY = y / (float)texHeight;
                    
                    // Check if point is in triangle
                    // Triangle: base at top (y=1), point at bottom center (y=0, x=0.5)
                    float leftEdge = normX * 2f; // 0 to 2
                    float rightEdge = (1f - normX) * 2f; // 2 to 0
                    
                    if (normY >= 1f - Mathf.Min(leftEdge, rightEdge))
                    {
                        pixels[y * texWidth + x] = color;
                    }
                }
            }
        }
        else
        {
            // Vertical sawtooth pattern
            for (int y = 0; y < texHeight; y++)
            {
                for (int x = 0; x < texWidth; x++)
                {
                    int toothIndex = y / 32;
                    int yInTooth = y % 32;
                    
                    // Normalized position in tooth (0 to 1)
                    float normX = x / (float)texWidth;
                    float normY = yInTooth / 32f;
                    
                    // Check if point is in triangle
                    // Triangle: base at right (x=1), point at left center (x=0, y=0.5)
                    float topEdge = normY * 2f; // 0 to 2
                    float bottomEdge = (1f - normY) * 2f; // 2 to 0
                    
                    if (normX >= 1f - Mathf.Min(topEdge, bottomEdge))
                    {
                        pixels[y * texWidth + x] = color;
                    }
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, texWidth, texHeight), new Vector2(0.5f, 0.5f), 32f);
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
                
                if (distance <= 30f)
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
        
        return Sprite.Create(texture, new Rect(0, 0, 64f, 64f), new Vector2(0.5f, 0.5f), 64f);
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
        
        return Sprite.Create(texture, new Rect(0, 0, 32f, 32f), new Vector2(0.5f, 0.5f), 32f);
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
                
                if (x <= triangleWidth * 32f)
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
        
        return Sprite.Create(texture, new Rect(0, 0, 32f, 32f), new Vector2(0f, 0.5f), 32f);
    }
    
    private static Sprite CreateOvalSprite(Color color)
    {
        Texture2D texture = new Texture2D(64, 64);
        Color[] pixels = new Color[64 * 64];
        
        // Draw an oval (ellipse)
        for (int y = 0; y < 64; y++)
        {
            for (int x = 0; x < 64; x++)
            {
                float dx = (x - 32) / 32f; // Normalize to -1 to 1
                float dy = (y - 32) / 24f; // Different radius for oval (wider than tall)
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                
                if (distance <= 1f)
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
        
        return Sprite.Create(texture, new Rect(0, 0, 64f, 64f), new Vector2(0.5f, 0.5f), 64f);
    }
}
