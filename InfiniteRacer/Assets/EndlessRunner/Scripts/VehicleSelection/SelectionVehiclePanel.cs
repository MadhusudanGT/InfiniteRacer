using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectionVehiclePanel : MonoBehaviour
{
    [SerializeField] private VehicleType vehicleType;
    [SerializeField] private Color enabledPanel, disabledPanel;
    [SerializeField] private TMP_Text vehicleName;
    [SerializeField] private TMP_Text vehicleFirstName;
    [SerializeField] private Image _selectedPanel,_vehicleImg;
    [SerializeField] private Button btnClick;
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        EventManager.SelectedVehiclePanel += SelectedPanel;
        btnClick.onClick.AddListener(SelectedVehicle);
    }

    private void OnDisable()
    {
        EventManager.SelectedVehiclePanel -= SelectedPanel;
        btnClick.onClick.RemoveListener(SelectedVehicle);
    }

    private void Start()
    {
        transform.localScale = Vector3.one;
    }

    void SelectedPanel(VehicleType vehType)
    {
        if (vehicleType == vehType)
        {
            _selectedPanel.color = enabledPanel;
        }
        else
        {
            _selectedPanel.color = disabledPanel;
        }
    }

    public void InitData(VehicleSelectionData vehicleSelectionData)
    {
        _vehicleImg.sprite = vehicleSelectionData.vehicleSprite;
        vehicleType = vehicleSelectionData.vehicleType;
        vehicleName.SetText(vehicleSelectionData.vehicleName);
        vehicleFirstName.SetText(vehicleSelectionData.vehicleName.Substring(0, 1));
        if (vehicleSelectionData.IsSelected)
        {
            SelectedVehicle();
        }
    }

    void SelectedVehicle()
    {
        EventManager.SelectedVehiclePanel?.Invoke(vehicleType);
    }
}
