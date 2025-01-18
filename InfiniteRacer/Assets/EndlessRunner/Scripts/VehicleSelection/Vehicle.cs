using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private VehicleType vehicleType;
    [SerializeField] private Vector3 _spawnPos = Vector3.zero;

    public void InitVehicle(Vector3 spawnPos)
    {
        _spawnPos = spawnPos;
        transform.localPosition = spawnPos;
    }

    private void OnEnable()
    {
        transform.localPosition = _spawnPos;
    }
}
