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
    public VehicleType vehicleType;
    public Vehicle vehiclePrefab;
    public Vector3 spawnPos;
    public string vehicleName;
    public float vehicleSpeed;
    public bool IsSelected;
    public Sprite vehicleSprite;

    public void IsSelectedVehicle(VehicleType vehType)
    {
        if (vehType == this.vehicleType)
        {
            this.IsSelected = true;
        }
        else
        {
            this.IsSelected = false;
        }
    }
}

public enum VehicleType
{
    NONE,
    AEROPLANE,
    CAR
}