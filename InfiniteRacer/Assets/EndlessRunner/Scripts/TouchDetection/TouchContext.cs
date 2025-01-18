using UnityEngine;

public class TouchContext
{
    private ITouchDetectionStrategy _strategy;

    public TouchContext(ITouchDetectionStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(ITouchDetectionStrategy strategy)
    {
        _strategy = strategy;
    }

    public bool ExecuteDetection(out bool isRightSide)
    {
        return _strategy.AreThreeTouchCount(out isRightSide);
    }
}
