using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundGenerator : MonoBehaviour
{
    [SerializeField] private EndlessTilesData tilesData;
    private PoolManagerGen poolManager;
    private Camera mainCamera;
    List<PlatformTile> spawnedTiles = new List<PlatformTile>();

    [HideInInspector]
    public bool gameOver = false;
    static bool gameStarted = false;
    float score = 0;

    public static GroundGenerator instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        ManagerRegistry.Register<GroundGenerator>(this);
        mainCamera = Camera.main;
    }

    void Start()
    {
        poolManager = ManagerRegistry.Get<PoolManagerGen>();
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

    // Update is called once per frame
    void Update()
    {
        if (!gameOver && gameStarted)
        {
            transform.Translate(-spawnedTiles[0].transform.forward * Time.deltaTime * (tilesData.movingSpeed + (score / 500)), Space.World);
            score += Time.deltaTime * tilesData.movingSpeed;
        }

        if (mainCamera.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < 0)
        {
            PlatformTile tileTmp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTmp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTmp.startPoint.localPosition;
            spawnedTiles.Add(tileTmp);
        }

        if (gameOver || !gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (gameOver)
                {
                    //Restart current scene
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                }
                else
                {
                    //Start the game
                    gameStarted = true;
                }
            }
        }
    }
}