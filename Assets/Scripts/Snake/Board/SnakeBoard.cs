using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SnakeBoard : MonoBehaviour
{
    [field: SerializeField]
    public Tilemap Tilemap { get; private set; }
    public List<Snake> Snakes { get; private set; } = new List<Snake>();
    public List<SnakeTile> SnakeTiles { get; private set; } = new List<SnakeTile>();

    public List<string> WordList { get; private set; } = new List<string>()
    {
        "Слово",
        "Не",
        "Воробей"
    };

    public int Width => _boardSize.x;
    public int Height => _boardSize.y;

    [SerializeField]
    private Vector2Int _boardSize;

    [SerializeField]
    private Vector2Int _playerSpawnPosition;

    [SerializeField]
    private int _playerSpawnSnakeLength = 3;

    [SerializeField] private Snake _playerSnake;

    [SerializeField] private GameObject _snakePrefab;

    [SerializeField] private Tile _commonBotTile;
    [SerializeField] private Tile _commonPlayerTile;

    public Tile CommonBotTile => _commonBotTile;
    public Tile CommonPlayerTile => _commonPlayerTile;

    private Vector2Int _calculatedMinBoundPoint;
    private Vector2Int _calculatedMaxBoundPoint;

    readonly Vector3Int[] directions = new Vector3Int[]
{
            new Vector3Int(0, 1, 0),   // Up
            new Vector3Int(1, 0, 0),   // Right
            new Vector3Int(0, -1, 0),  // Down
            new Vector3Int(-1, 0, 0)   // Left
    };

    private void Awake()
    {
        bool boardWidthOdd = _boardSize.x % 2 == 0;
        bool boardHeightOdd = _boardSize.y % 2 == 0;
        int xAppendix = boardWidthOdd ? -1 : 0;
        int yAppendix = boardHeightOdd ? -1 : 0;
        _calculatedMinBoundPoint = new Vector2Int(-_boardSize.x / 2, -_boardSize.y / 2);
        _calculatedMaxBoundPoint = new Vector2Int(_boardSize.x / 2 + xAppendix, _boardSize.y / 2 + yAppendix);

        Tilemap.tileAnchor = new Vector3(
            boardWidthOdd ? 0.5f : 0,
            boardHeightOdd ? 0.5f : 0,
            0);
    }

    private void OnEnable()
    {
        // InstantiateNewBotSnake(_playerSpawnPosition - new Vector2Int(2, 0), _playerSpawnSnakeLength);
        InstantiatePlayerSnake(_playerSnake, _playerSpawnPosition, _playerSpawnSnakeLength); // Dont call in awake
                                                                                  //        InstantiateNewPlayerSnake(_playerSpawnPosition, _playerSpawnSnakeLength); // Dont call in awake
        InstantiateNewBotSnake(_playerSpawnPosition + new Vector2Int(2,0), _playerSpawnSnakeLength, 1);
    }

    public void InstantiatePlayerSnake(Snake snake, Vector2Int spawnPosition, int length)
    {
        var controller = snake.gameObject.GetComponent<PlayerSnakeController>();
        snake.Initialize(CommonPlayerTile, this, controller, spawnPosition, length);
    }

    //public void InstantiateNewPlayerSnake(Vector2Int spawnPosition, int length)
    //{
    //    var instance = GameObject.Instantiate(_snakePrefab, transform);
    //    var snake = instance.GetComponent<Snake>();

    //    GameObject obj = new GameObject();
    //    var controller = obj.AddComponent<PlayerSnakeController>();

    //    snake.Initialize(CommonPlayerTile, this, controller, spawnPosition, length);
    //}

    public void InstantiateNewBotSnake(Vector2Int spawnPosition, int length, int thinkAheadDepth = 10)
    {
        var instance = GameObject.Instantiate(_snakePrefab, transform);
        var snake = instance.GetComponent<Snake>();
        var controller = new BotSnakeController(snake, this, thinkAheadDepth);
        snake.Initialize(CommonBotTile, this, controller, spawnPosition, length);
    }

    private void MoveTile(Vector3Int position, Vector3Int newPosition)
    {
        var tile = Tilemap.GetTile(position);
        Tilemap.SetTile(newPosition, tile);
        Tilemap.SetTile(position, null);
    }

    private void MoveTile(SnakeTile snakeTile, Vector2Int newPosition, string newLetter = null)
    {
        if (IsPositionOccupied(newPosition))
        {
            throw new Exception("Cannot move tile to occupied position");
        }

        if (!IsPositionWithinBounds(newPosition))
        {
            throw new Exception("Cannot move tile to position out of bounds");
        }

        // Optionally update the text
        if (newLetter != null)
        {
            snakeTile.UpdateText(newLetter);
        }
        
        // Move the tile
        Tilemap.SetTile(newPosition.ToVector3Int(), snakeTile.CustomTile);
        Tilemap.SetTile(snakeTile.Position.ToVector3Int(), null);

        // Update position
        snakeTile.Position = newPosition;
    }

    public Vector3Int? FindFreeAdjacentPosition(Vector3Int newPosition, Vector3Int? prefferedDirection)
    {
        if (prefferedDirection.HasValue)
        {
            Vector3Int adjacentPosition = newPosition + prefferedDirection.Value;
            Vector2Int adjacentVector2 = new Vector2Int(adjacentPosition.x, adjacentPosition.y);
            if (IsPositionWithinBounds(adjacentVector2) && !IsPositionOccupied(adjacentVector2))
            {
                return adjacentPosition;
            }
        }

        foreach (var direction in directions)
        {
            Vector3Int adjacentPosition = newPosition + direction;

            // Convert to Vector2Int for boundary and occupation checks
            Vector2Int adjacentVector2 = new Vector2Int(adjacentPosition.x, adjacentPosition.y);

            // Check if the adjacent position is within bounds and not occupied
            if (IsPositionWithinBounds(adjacentVector2) && !IsPositionOccupied(adjacentVector2))
            {
                return adjacentPosition; // Return the first free adjacent position
            }
        }

        return null; // Return null if no free adjacent position is found
    }

    public Vector3Int? FindFreeSpotWithNFreeSpaces(int requiredFreeSpaces)
    {
        for (int x = _calculatedMinBoundPoint.x; x <= _calculatedMaxBoundPoint.x; x++)
        {
            for (int y = _calculatedMinBoundPoint.y; y <= _calculatedMaxBoundPoint.y; y++)
            {
                Vector3Int currentPosition = new Vector3Int(x, y, 0);
                if (Tilemap.GetTile(currentPosition) != null)
                    continue; // Skip if the current tile is occupied

                int freeCount = 0;

                foreach (var direction in directions)
                {
                    Vector3Int neighborPosition = currentPosition + direction;
                    if (Tilemap.GetTile(neighborPosition) == null)
                    {
                        freeCount++;
                    }
                }

                if (freeCount >= requiredFreeSpaces)
                {
                    return currentPosition; // Return the first position that matches the criteria
                }
            }
        }

        return null; // Return null if no suitable position is found
    }

    public void PurgeAll()
    {

    }

    public bool IsPositionWithinBounds(Vector2Int position)
    {
        return position.x >= _calculatedMinBoundPoint.x && position.x <= _calculatedMaxBoundPoint.x &&
               position.y >= _calculatedMinBoundPoint.y && position.y <= _calculatedMaxBoundPoint.y;
    }

    public bool IsPositionOccupied(Vector2Int position)
    {
        return Tilemap.GetTile(position.ToVector3Int()) != null;
        //return Tilemap.HasTile(position.ToVector3Int());
    }

    public bool TryMoveSnake(Snake snake, Vector2Int nextPosition)
    {
        if (!IsPositionWithinBounds(nextPosition))
        {
            Debug.Log("Out of bounds");
            return false;
        }

        if (IsPositionOccupied(nextPosition))
        {
            Debug.Log("Occupied");
            return false;
        }

        MoveSnake(snake, nextPosition);

        return true;
    }

    private void MoveSnake(Snake snake, Vector2Int nextPosition)
    {
        Vector2Int previousPosition;

        foreach (var tile in snake.Tiles)
        {
            previousPosition = tile.Position;

            // Optionally update text (e.g., if text depends on position or order)
            string newText = tile.Order.ToString(); // Example: update text to show the order
            MoveTile(tile, nextPosition, newText);

            nextPosition = previousPosition;
        }
    }

    internal void SetColor(Tile customTile, Vector2Int position, Color color)
    {
        Tilemap.SetTile(position.ToVector3Int(), null);
        customTile.color = color;
        Tilemap.SetTile(position.ToVector3Int(), customTile);
    }

    //private void MoveSnake(Snake snake, Vector2Int nextPosition)
    //{
    //    Vector2Int buffer;
    //    foreach (var item in snake.Tiles)
    //    {
    //        buffer = item.Position;
    //        MoveTile(item, nextPosition);
    //        nextPosition = buffer;
    //    }
    //}
}
