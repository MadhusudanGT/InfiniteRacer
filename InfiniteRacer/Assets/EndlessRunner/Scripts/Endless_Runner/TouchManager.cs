using DG.Tweening.Core.Easing;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private TouchContext _touchContext;
    private MoveDirections currentMoveDirection = MoveDirections.Center;
    private GameManager gameManager;
    private bool gamePaused = false;

    void Start()
    {
        _touchContext = new TouchContext(new ThreeTouchDetectionStrategy());
        gameManager = ManagerRegistry.Get<GameManager>();
    }

    void Update()
    {
        if (gameManager == null) return;

        var touches = Input.touches;

        if (touches.Length <= 0)
        {
            return;
        }

        if (touches.Length == 4 && !gamePaused)
        {
            gamePaused = true;
            CheckForFourTouch(touches);
            return;
        }

        if (touches.Length != 4 && touches.Length != 0)
        {
            gamePaused = false;
        }

        if (gameManager.CurrentState == GameState.Paused)
        {
            return;
        }


        if (_touchContext.ExecuteDetection(out bool isRightSide))
        {
            if (touches.Length == 3)
            {
                CheckForThreeTouch(touches, isRightSide);
                return;
            }
        }
        else
        {
            Debug.LogWarning("Exactly 3 touch points are required to detect a triangle.");
        }
    }

    void CheckForThreeTouch(Touch[] touches, bool isRightSide)
    {
        bool isLeftTriangle = TriangleUtility.AreTouchesInTriangle(touches, true);
        bool isRightTriangle = TriangleUtility.AreTouchesInTriangle(touches, false);

        if (ValidateIsRightAngleTriable(isRightSide, isRightTriangle))
        {
            SetMoveDirection(MoveDirections.Right);
            //Debug.Log("...." + isRightSide + "....IS RIGHT SIDE TRIANGLE FORMATED IN RIGHT SIDE...." + isRightTriangle + ".....IS LEFT SIDE..." + isLeftTriangle);
        }
        else if (ValidateIsLeftAngleTriable(isRightSide, isLeftTriangle))
        {
            SetMoveDirection(MoveDirections.Left);
            //Debug.Log(!isRightSide + "....IS LEFT SIDE TRIANGLE FORMATED IN LEFT SIDE...." + isLeftTriangle + "....IS RIGHT SIDE..." + isRightTriangle);
        }
        else
        {
            SetMoveDirection(MoveDirections.Center);
        }
    }

    void CheckForFourTouch(Touch[] touches)
    {
        bool IsTriangleFormted = TriangleUtility.IsValidSquare(touches);
        if (IsTriangleFormted)
        {
            ToggleGameState();
        }
        else
        {
            SetMoveDirection(MoveDirections.Center);
        }
    }

    bool ValidateIsRightAngleTriable(bool isRightSide, bool isRightTriangle)
    {
        return isRightSide == true && isRightTriangle == true;
    }

    bool ValidateIsLeftAngleTriable(bool isRightSide, bool isLeftTriangle)
    {
        return !isRightSide == true && isLeftTriangle == true;
    }

    private void SetMoveDirection(MoveDirections direction)
    {
        if (currentMoveDirection != direction)
        {
            currentMoveDirection = direction;
            EventManager.MoveDirection?.Invoke(direction);
        }
    }
    private void ToggleGameState()
    {
        if (gameManager.CurrentState == GameState.Running)
        {
            gameManager.CurrentState = GameState.Paused;
        }
        else
        {
            gameManager.CurrentState = GameState.Running;
        }
    }
}
