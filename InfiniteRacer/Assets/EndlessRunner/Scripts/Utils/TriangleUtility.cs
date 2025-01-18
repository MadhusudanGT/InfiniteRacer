using System.Linq;
using UnityEngine;

public static class TriangleUtility
{
    public static bool AreTouchesInTriangle(Touch[] touches, bool isLeftTriangle)
    {
        if (touches.Length < 3) return false;

        var sortedTouches = touches.OrderBy(t => t.position.x).ToArray();

        if (isLeftTriangle)
        {
            float baseX = Mathf.Abs(sortedTouches[0].position.x - sortedTouches[1].position.x);
            float heightY = Mathf.Abs(sortedTouches[0].position.y - sortedTouches[2].position.y);

            return baseX < Screen.width * 0.1f && heightY < Screen.height * 0.2f;
        }
        else
        {
            float baseX = Mathf.Abs(sortedTouches[2].position.x - sortedTouches[1].position.x);
            float heightY = Mathf.Abs(sortedTouches[2].position.y - sortedTouches[0].position.y);

            return baseX < Screen.width * 0.1f && heightY < Screen.height * 0.2f;
        }
    }

    public static bool IsValidSquare(Touch[] touches)
    {
        if (touches.Length != 4) return false;

        float[] xPositions = touches.Select(t => t.position.x).ToArray();
        float[] yPositions = touches.Select(t => t.position.y).ToArray();

        float xRange = xPositions.Max() - xPositions.Min();
        float yRange = yPositions.Max() - yPositions.Min();

        return Mathf.Abs(xRange - yRange) < Screen.width * 0.1f;
    }
}
