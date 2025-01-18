using System.Collections.Generic;
using UnityEngine;

public class VehicleDataLoader : MonoBehaviour
{
    public static VehicleDataLoader instance;
    public List<VehicleSelectionData> vehicleDataList;

    private void Awake()
    {
        LoadVehicleData();
        instance = this;
    }

    private void LoadVehicleData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("VehicleData");
        if (jsonFile == null)
        {
            Debug.LogError("VehicleData.json not found in Resources!");
            return;
        }
        vehicleDataList = JsonUtility.FromJson<VehicleSelectionDataWrapper>(jsonFile.text).vehicles;
        foreach (var vehicleData in vehicleDataList)
        {
            vehicleData.LoadResources();
            vehicleData.UpdateVehicleType();
        }
    }
}

[System.Serializable]
public class VehicleSelectionDataWrapper
{
    public List<VehicleSelectionData> vehicles;
}
