using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleSelection", menuName = "Scriptable Objects/VehicleSelection")]
public class VehicleSelection : ScriptableObject
{
    public List<VehicleSelectionData> _vehiclesData;
}

[System.Serializable]
public class VehicleSelectionData
{
    public string vehicleTypeString;
    public VehicleType vehicleType;

    public Vector3 spawnPos;
    public string vehicleName;
    public float vehicleSpeed;
    public bool isSelected;
    public string vehiclePrefabPath;
    public string vehicleSpritePath;

    public Vehicle vehiclePrefab;
    public Sprite vehicleSprite;

    public void LoadResources()
    {
        if (!string.IsNullOrEmpty(vehiclePrefabPath))
        {
            vehiclePrefab = Resources.Load<Vehicle>(vehiclePrefabPath);
        }
        if (!string.IsNullOrEmpty(vehicleSpritePath))
        {
            vehicleSprite = Resources.Load<Sprite>(vehicleSpritePath);
        }
    }

    public void UpdateVehicleType()
    {
        if (System.Enum.TryParse(vehicleTypeString, true, out VehicleType parsedType))
        {
            vehicleType = parsedType;
        }
        else
        {
            Debug.LogWarning($"Unknown vehicleType: {vehicleType}");
        }
    }
}

public enum VehicleType
{
    NONE,
    CAR,
    AEROPLANE,
}