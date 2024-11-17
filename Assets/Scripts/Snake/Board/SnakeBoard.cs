using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    [SerializeField] private GameObject _snakePrefab;

    private Vector2Int _calculatedMinBoundPoint;
    private Vector2Int _calculatedMaxBoundPoint;
    
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
        
        InstantiateSnake();
    }
    
    public void InstantiateSnake()
    {
        var instance = GameObject.Instantiate(_snakePrefab, transform);
        var snake = instance.GetComponent<Snake>();

        Tilemap.SetTile(_calculatedMinBoundPoint.ToVector3Int(), snake.tile);
        Tilemap.SetTile(_calculatedMaxBoundPoint.ToVector3Int(), snake.tile);
        // snake.Initialize(this, _playerSpawnPosition, _playerSpawnSnakeLength);
    }
    
    public void PurgeAll()
    {
        
    }
    
    public void Add(Snake snake)
    {
        
    }
    
    public void Remove(Snake snake)
    {
        
    }

    public void Remove(SnakeTile tile)
    {
        
    }
    
    public bool IsPositionWithinBounds(Vector2Int position)
    {
        return false;
    }
    
    public bool IsPositionOccupied(Vector2Int position)
    {
        return false;
    }
}
