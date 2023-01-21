using UnityEngine;

static class ChooseRandomPositon
{
    public static Vector3 ChooseSpawningPosition((int, int) xRange, (int, int) yRange)
    {
        int xPos = Random.Range(xRange.Item1, xRange.Item2 + 1);
        int yPos = Random.Range(yRange.Item1, yRange.Item2 + 1);
        return new Vector3(xPos, yPos, 0);
    }
}