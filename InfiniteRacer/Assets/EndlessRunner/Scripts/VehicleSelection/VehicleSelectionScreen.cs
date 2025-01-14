using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

public class VehicleSelectionScreen : MonoBehaviour
{
    [SerializeField] private VehicleSelection vehicleSelectionData;
    [SerializeField] private Transform parent, vehicleSelectionPanelParent;
    [SerializeField] private VehicleType defaultVehicleType = VehicleType.CAR;
    [SerializeField] private List<SelectionVehiclePanel> listOfSelectionVehiclePanel;
    [SerializeField] private List<SpawnVehicle> _spawnVehicle;
    private PoolManagerGen poolManager;

    private void OnEnable()
    {
        EventManager.SelectedVehiclePanel += SelectedPanel;
    }

    private void OnDisable()
    {
        EventManager.SelectedVehiclePanel -= SelectedPanel;
    }

    void SelectedPanel(VehicleType vehType)
    {
        foreach (var vehicleData in vehicleSelectionData._vehiclesData)
        {
            vehicleData.IsSelectedVehicle(vehType);
        }

        foreach (var spawnVehicle in _spawnVehicle)
        {
            if (spawnVehicle.spawnedVehicle != null)
            {
                bool isActive = spawnVehicle.vehicleType == vehType;
                spawnVehicle.spawnedVehicle.transform.gameObject.SetActive(isActive);
            }
        }
    }

    private void Start()
    {
        poolManager = ManagerRegistry.Get<PoolManagerGen>();
        SpawnThePanel();
    }

    async Task SpawnThePanel()
    {
        await Task.Delay(1000);
        SpawnIt(vehicleSelectionData._vehiclesData);
    }

    public void SpawnIt(List<VehicleSelectionData> vehicalPanel)
    {
        if (vehicalPanel == null || vehicalPanel.Count == 0)
        {
            Debug.LogWarning("No vehicalPanel provided to display on the vehiclePanel.");
            return;
        }

        int displayCount = Mathf.Min(vehicalPanel.Count, 5);

        while (listOfSelectionVehiclePanel.Count < displayCount)
        {
            listOfSelectionVehiclePanel.Add(null);
        }

        for (int i = 0; i < displayCount; i++)
        {
            if (i < listOfSelectionVehiclePanel.Count && listOfSelectionVehiclePanel[i] != null)
            {
                listOfSelectionVehiclePanel[i].InitData(vehicalPanel[i]);
                SetVehicalData(listOfSelectionVehiclePanel[i].transform);
            }
            else
            {
                SpawnVehicleData(vehicalPanel[i], i);
            }
        }

        for (int i = displayCount; i < listOfSelectionVehiclePanel.Count; i++)
        {
            if (listOfSelectionVehiclePanel[i] != null)
            {
                listOfSelectionVehiclePanel[i].gameObject.SetActive(false);
            }
        }
    }

    void SpawnVehicleData(VehicleSelectionData vehicleDatas, int rank)
    {
        if (poolManager == null)
        {
            Debug.LogError("PoolManager is not initialized. Cannot spawn vehicle data.");
            return;
        }

        var vehiclePanel = poolManager.GetObject<SelectionVehiclePanel>(PoolManagerKeys.SELECTION_PANEL);
        if (vehiclePanel == null)
        {
            Debug.LogError("Failed to retrieve vehiclePanel object from the pool.");
            return;
        }

        vehiclePanel.InitData(vehicleDatas);
        SetVehicalData(vehiclePanel.transform);

        SpawnVehicles(vehicleDatas);

        if (rank < listOfSelectionVehiclePanel.Count)
        {
            listOfSelectionVehiclePanel[rank] = vehiclePanel;
        }
        else
        {
            listOfSelectionVehiclePanel.Add(vehiclePanel);
        }
    }

    void SetVehicalData(Transform itemTransform)
    {
        if (itemTransform == null)
        {
            Debug.LogError("vehiclePanel item transform is null.");
            return;
        }

        itemTransform.SetParent(vehicleSelectionPanelParent);
    }

    void SpawnVehicles(VehicleSelectionData vehicleDatas)
    {
        SpawnVehicle spawnData = _spawnVehicle.Find(item => item.vehicleType == vehicleDatas.vehicleType);
        Transform vehicle = Instantiate(spawnData.vehiclePrefab, vehicleDatas.spawnPos, Quaternion.identity) as Transform;
        vehicle.SetParent(parent);
        vehicle.localScale = Vector3.one;
        Vehicle veh = vehicle.GetComponent<Vehicle>();
        if (veh != null)
        {
            spawnData.spawnedVehicle = veh;
            if (vehicleDatas.IsSelected)
            {
                vehicle.gameObject.SetActive(true);
            }
        }
    }
}

[System.Serializable]
public class SpawnVehicle
{
    public VehicleType vehicleType;
    public Transform vehiclePrefab;
    public Vehicle spawnedVehicle;
}