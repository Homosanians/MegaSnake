using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TetrominoData
{
    public TetrominoType TetrominoType;
    public Tile Tile;
    public string[] Letters;
    public Vector2Int[] Cells { get; private set; }

    public Vector2Int[,] WallKicks { get; private set; }

    public void Initialize() 
    {
        this.Cells = Data.Cells[this.TetrominoType];
        WallKicks = Data.WallKicks[this.TetrominoType];
    }
}
