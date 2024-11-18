using UnityEngine;

public static class Vector3IntExtensions
{
    public static Vector2Int ToVector2Int(this Vector3Int vector3Int)
    {
        return (Vector2Int)vector3Int;
    }

}