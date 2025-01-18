using UnityEngine;

public interface ITouchDetectionStrategy
{
    bool AreThreeTouchCount(out bool isRightSide); 
}

public class ThreeTouchDetectionStrategy : ITouchDetectionStrategy
{
    public bool AreThreeTouchCount(out bool isRightSide)
    {
        isRightSide = false;

        if (Input.touchCount != 3)
            return false;

        int leftCount = 0;
        int rightCount = 0;

        float screenMidpoint = Screen.width / 2f;

        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < screenMidpoint)
                leftCount++;
            else
                rightCount++;
        }

        if (leftCount == 3)
        {
            isRightSide = false;
            return true;
        }
        else if (rightCount == 3)
        {
            isRightSide = true;
            return true;
        }

        return false;
    }
}

