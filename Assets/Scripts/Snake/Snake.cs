using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Snake : MonoBehaviour
{
    public SnakeBoard Board { get; private set; }
    public List<SnakeTile> Tiles { get; private set; } = new List<SnakeTile>();
    private ISnakeController _controller;
    private bool _isDead = false;
    private Tile _commonTile;

    readonly List<Vector2Int> directions = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public void AddSnakeTile(int order, Vector2Int position, string letter)
    {
        var snakeTile = new SnakeTile(Board, this, order, position, letter, ScriptableObject.Instantiate(_commonTile));

        Tiles.Add(snakeTile);
        Board.Tilemap.SetTile(position.ToVector3Int(), snakeTile.CustomTile);
    }

    public void AddSnakeTileToTail(string letter)
    {
        Vector2Int tailPosition = Tiles[^1].Position; // Get the position of the tail

        AddSnakeTile(Tiles.Count, tailPosition, letter);
    }

    public void Initialize(Tile commonTile, SnakeBoard snakeBoard, ISnakeController controller, Vector2Int position, char[] letters)
    {
        _commonTile = commonTile;
        Board = snakeBoard;
        _controller = controller;

        SnakeOrchestrator.Instance.Register(this);

        Vector2Int currentPosition = position;

        // Create and set the body tiles
        for (int i = 0; i < letters.Length; i++)
        {
            Vector3Int? nextPosition = Board.FindFreeAdjacentPosition(currentPosition.ToVector3Int(), prefferedDirection: new Vector3Int(0, -1, 0));
            if (nextPosition.HasValue)
            {
                AddSnakeTile(i, nextPosition.Value.ToVector2Int(), letters[i].ToString());
                currentPosition = nextPosition.Value.ToVector2Int();
            }
            else
            {
                throw new System.Exception("Snake can't extend further at this point.");
            }
        }
    }

    public void Die()
    {
        Debug.LogWarning("Snake died");

        SnakeOrchestrator.Instance.Deregister(this);

        _isDead = true;

        foreach (var item in Tiles)
        {
            item.ChangeState(SnakeTileState.Orphaned);
        }

        Board.OrphanedTiles.AddRange(Tiles);
        Tiles.Clear();

        Destroy(this);
    }

    public void Move()
    {
        if (_isDead)
        {
            return;
        }

        var headPosition = CalculateHeadPosition();
        var currentDirection = CalculateCurrentDirection();

        var decision = _controller.MakeDecision(headPosition, currentDirection);

        Vector2Int nextPosition = CalculateNextPosition(decision, headPosition, currentDirection);

        // Pass the next position to the board to attempt movement
        bool isSuccess = Board.TryMoveSnake(this, nextPosition);

        if (!isSuccess)
            Die();
    }

    private Vector2Int CalculateHeadPosition()
    {
        Vector2Int headPosition = Tiles[0].Position;
        return headPosition;
    }

    private Vector2Int CalculateCurrentDirection()
    {
        Vector2Int currentDirection = Tiles.Count > 1
            ? Tiles[0].Position - Tiles[1].Position // Calculate direction from head to second tile
            : Vector2Int.up; // Default to upward movement for single-tile snakes

        return currentDirection;
    }

    private Vector2Int CalculateNextPosition(SnakeAction decision, Vector2Int headPosition, Vector2Int currentDirection)
    {
        Vector2Int nextDirection = decision switch
        {
            SnakeAction.MoveForward => currentDirection,
            SnakeAction.TurnLeft => new Vector2Int(-currentDirection.y, currentDirection.x),
            SnakeAction.TurnRight => new Vector2Int(currentDirection.y, -currentDirection.x),
            _ => throw new System.ArgumentException("Invalid SnakeAction provided.")
        };

        // Debug.Log($"current dir {currentDirection} - head {headPosition} - moving {decision} - next pos {headPosition + nextDirection}");

        // Calculate the next position
        return headPosition + nextDirection;
    }
}
