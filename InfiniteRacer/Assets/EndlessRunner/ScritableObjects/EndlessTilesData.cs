using UnityEngine;

[CreateAssetMenu(fileName = "EndlessTilesData", menuName = "Scriptable Objects/EndlessTilesData")]
public class EndlessTilesData : ScriptableObject
{
    public Vector3 spawnPoint; //Point from where ground tiles will start
    public PlatformTile tilePrefab;
    public float movingSpeed = 12;
    public int tilesToPreSpawn = 15; //How many tiles should be pre-spawned
}
