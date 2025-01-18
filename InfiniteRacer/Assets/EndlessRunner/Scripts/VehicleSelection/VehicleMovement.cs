using DG.Tweening;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [SerializeField] private float laneOffset = 3f;
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
        transform.DOKill();

        Vector3 targetPosition = transform.position;

        switch (moveDirections)
        {
            case MoveDirections.Right:
                targetPosition = new Vector3(laneOffset, 0, 0);
                //Debug.Log("VEHICLE MOVE DIRECTION RIGHT... " + moveDirections);
                break;

            case MoveDirections.Left:
                targetPosition = new Vector3(-laneOffset, 0, 0);
                //Debug.Log("VEHICLE MOVE DIRECTION LEFT... " + moveDirections);
                break;

            case MoveDirections.Center:
                targetPosition = new Vector3(0, 0, 0);
                //Debug.Log("VEHICLE MOVE DIRECTION CENTER... " + moveDirections);
                break;

            default:
                Debug.LogWarning("Unknown Move Direction... " + moveDirections);
                return;
        }

        transform.DOMove(targetPosition, 0.5f).SetEase(Ease.InOutSine);
    }

}
