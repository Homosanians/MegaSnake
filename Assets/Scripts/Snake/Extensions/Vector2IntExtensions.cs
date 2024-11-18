using UnityEngine;

public static class Vector2IntExtensions
{
    public static Vector3Int ToVector3Int(this Vector2Int vector2Int)
    {
        return (Vector3Int) vector2Int;
    }
}