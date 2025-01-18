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
}

public enum MoveDirections
{
    None,
    Right,
    Left,
    Center
}
