using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<GameState> UpdateGameState;
    public static Action<VehicleType> SelectedVehiclePanel;
    public static Action<MoveDirections> MoveDirection;
}

