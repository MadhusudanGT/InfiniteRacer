using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public static GroundGenerator Instance;

    [SerializeField] private EndlessTilesData tilesData;

    private GameManager gameManager;
    private PoolManagerGen poolManager;
    private Camera mainCamera;

    private List<PlatformTile> spawnedTiles = new List<PlatformTile>();
    private MoveDirections currentMoveDirection = MoveDirections.Center;
    private bool gestureActive = false; // Tracks if a gesture is currently active

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ManagerRegistry.Register(this);
        mainCamera = Camera.main;
    }

    private void Start()
    {
        InitializeComponents();
        PreSpawnTiles();
    }

    private void Update()
    {
        if (gameManager == null) return;

        if (gameManager.CurrentState == GameState.Running)
        {
            if (spawnedTiles.Count > 0)
            {
                MoveTiles();
                RecycleTiles();
            }
        }

        HandleInput();
    }

    private void InitializeComponents()
    {
        poolManager = ManagerRegistry.Get<PoolManagerGen>();
        gameManager = ManagerRegistry.Get<GameManager>();
    }

    private void PreSpawnTiles()
    {
        Vector3 spawnPosition = tilesData.spawnPoint;

        for (int i = 0; i < tilesData.tilesToPreSpawn; i++)
        {
            spawnPosition -= tilesData.tilePrefab.startPoint.localPosition;
            var platform = poolManager.GetObject<PlatformTile>(PoolManagerKeys.PLATFORM);

            platform.transform.position = spawnPosition;
            spawnPosition = platform.endPoint.position;

            platform.transform.SetParent(transform);
            spawnedTiles.Add(platform);
        }
    }

    private void MoveTiles()
    {
        float moveSpeed = tilesData.movingSpeed * Time.deltaTime;
        transform.Translate(-spawnedTiles[0].transform.forward * moveSpeed, Space.World);
    }

    private void RecycleTiles()
    {
        if (mainCamera.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < 0)
        {
            PlatformTile recycledTile = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);

            Vector3 newTilePosition = spawnedTiles[^1].endPoint.position - recycledTile.startPoint.localPosition;
            recycledTile.transform.position = newTilePosition;

            spawnedTiles.Add(recycledTile);
        }
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            gestureActive = true;

            if (Input.touchCount == 3)
            {
                DetectThreeFingerGesture();
            }
            else if (Input.touchCount == 4)
            {
                DetectFourFingerGesture();
            }
        }
        else
        {
            // Reset to center when no fingers are touching the screen
            if (gestureActive)
            {
                gestureActive = false;
                SetMoveDirection(MoveDirections.Center);
            }
        }
    }

    private void DetectThreeFingerGesture()
    {
        var touches = Input.touches;

        bool isLeftTriangle = AreTouchesInTriangle(touches, true);
        bool isRightTriangle = AreTouchesInTriangle(touches, false);

        if (isLeftTriangle)
        {
            SetMoveDirection(MoveDirections.Left);
        }
        else if (isRightTriangle)
        {
            SetMoveDirection(MoveDirections.Right);
        }
    }

    private void DetectFourFingerGesture()
    {
        var touches = Input.touches;

        if (AreTouchesInSquare(touches))
        {
            ToggleGameState();
        }
    }

    private void SetMoveDirection(MoveDirections direction)
    {
        if (currentMoveDirection != direction)
        {
            currentMoveDirection = direction;
            EventManager.MoveDirection?.Invoke(direction);
        }
    }

    private void ToggleGameState()
    {
        if (gameManager.CurrentState == GameState.Running)
        {
            gameManager.CurrentState = GameState.Paused;
        }
        else
        {
            gameManager.CurrentState = GameState.Running;
        }
    }

    private bool AreTouchesInTriangle(Touch[] touches, bool isLeftTriangle)
    {
        if (touches.Length < 3) return false;

        var sortedTouches = touches.OrderBy(t => t.position.x).ToArray();

        if (isLeftTriangle)
        {
            float baseX = Mathf.Abs(sortedTouches[0].position.x - sortedTouches[1].position.x);
            float heightY = Mathf.Abs(sortedTouches[0].position.y - sortedTouches[2].position.y);

            return baseX < Screen.width * 0.1f && heightY < Screen.height * 0.2f;
        }
        else
        {
            float baseX = Mathf.Abs(sortedTouches[2].position.x - sortedTouches[1].position.x);
            float heightY = Mathf.Abs(sortedTouches[2].position.y - sortedTouches[0].position.y);

            return baseX < Screen.width * 0.1f && heightY < Screen.height * 0.2f;
        }
    }

    private bool AreTouchesInSquare(Touch[] touches)
    {
        if (touches.Length != 4) return false;

        float[] xPositions = touches.Select(t => t.position.x).ToArray();
        float[] yPositions = touches.Select(t => t.position.y).ToArray();

        float xRange = xPositions.Max() - xPositions.Min();
        float yRange = yPositions.Max() - yPositions.Min();

        return Mathf.Abs(xRange - yRange) < Screen.width * 0.1f; // Adjust for screen size
    }
}

public enum MoveDirections
{
    None,
    Right,
    Left,
    Center
}
