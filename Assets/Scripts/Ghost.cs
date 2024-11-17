using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile Tile;
    public Board Board;
    public Piece TrackingPiece;

    public Tilemap Tilemap { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    public Vector3Int Position { get; private set; }

    private void Awake()
    {
        Tilemap = GetComponentInChildren<Tilemap>();
        Cells = new Vector3Int[4];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear() 
    {
        //TODO: FIX CLEANING
        /*for (var i = 0; i < Cells.Length; i++)
        {
            Vector3Int tilePosition = Cells[i] + Position;
            Tilemap.SetTile(tilePosition, null);
        }*/
        Tilemap.ClearAllTiles();
    }

    private void Copy() 
    {
        for (var i = 0; i < Cells.Length; i++) 
        {
            Cells = TrackingPiece.Cells;
        }
    }

    private void Drop() 
    {
        Vector3Int position = TrackingPiece.Position;

        int current = position.y;
        int bottom = -Board.BoardSize.y / 2 - 1;

        Board.Clear(TrackingPiece);

        for (var row = current; row >= bottom; row--)
        {
            position.y = row;

            if (Board.IsValidPosition(TrackingPiece, position))
            {
                Position = position;
            }
            else 
            {
                break;
            }
        }

        Board.Set(TrackingPiece);
    }

    private void Set() 
    {
        for (var i = 0; i < Cells.Length; i++)
        {
            Vector3Int tilePosition = Cells[i] + Position;
            Tilemap.SetTile(tilePosition, Tile);
        }
    }
}
