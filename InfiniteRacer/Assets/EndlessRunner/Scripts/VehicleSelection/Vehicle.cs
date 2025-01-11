using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private VehicleType vehicleType;
    [SerializeField] 
private float laneOffset = 3f;
    private void OnEnable()
    {
        EventManager.MoveDirection += MoveTowards;
    }

    private void OnDisable()
    {
        EventManager.MoveDirection -= MoveTowards;
    }

    void MoveTowards(MoveDirections moveDirections)
    {
        if (moveDirections == MoveDirections.Right)
        {   
            transform.position = new Vector3(laneOffset, transform.position.y, transform.position.z);
            Debug.Log("VEHICLE MOVE DIRECTION RIGHT..." + moveDirections);
        }
        else if (moveDirections == MoveDirections.Left)
        {
            transform.position = new Vector3(-laneOffset, transform.position.y, transform.position.z);
            Debug.Log("VEHICLE MOVE DIRECTION LEFT..." + moveDirections);
        }
        else if (moveDirections == MoveDirections.Center)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            //Debug.Log("VEHICLE MOVE DIRECTION CENTER..." + moveDirections);
        }
        else
        {
            //Debug.Log("VEHICLE MOVE DIRECTION..." + moveDirections);
        }
    }
}
