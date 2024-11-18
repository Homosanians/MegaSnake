using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Snake : MonoBehaviour
{
    public SnakeBoard Board { get; private set; }
    public List<SnakeTile> Tiles { get; private set; } = new List<SnakeTile>();
    private ISnakeController _controller;

    public void AddSnakeTile(int order, Vector2Int position, string letter)
    {
        var snakeTile = new SnakeTile(order, position, letter, ScriptableObject.Instantiate(Board.CommonTile));

        Tiles.Add(snakeTile);
        Board.Tilemap.SetTile(position.ToVector3Int(), snakeTile.CustomTile);
    }

    public void Initialize(SnakeBoard snakeBoard, Vector2Int position, int length)
    {
        Board = snakeBoard;
        _controller = new BotSnakeController(this, Board, 10);

        SnakeOrchestrator.Instance.Register(this);

        // Create and set the head tile
        AddSnakeTile(0, position, "0");

        Vector2Int currentPosition = position;

        // Create and set the body tiles
        for (int i = 1; i < length; i++)
        {
            Vector3Int? nextPosition = Board.FindFreeAdjacentPosition(currentPosition.ToVector3Int(), prefferedDirection: new Vector3Int(0, -1, 0));
            if (nextPosition.HasValue)
            {
                AddSnakeTile(i, nextPosition.Value.ToVector2Int(), i.ToString());
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
        Destroy(this);
    }

    public void Move()
    {
        var decision = _controller.MakeDecision();

        // Calculate the next position based on the decision
        Vector2Int nextPosition = CalculateNextPosition(decision);

        // Pass the next position to the board to attempt movement
        bool isSuccess = Board.TryMoveSnake(this, nextPosition);

        if (!isSuccess)
            Die();

        Debug.Log($"Moving snake {decision} (count {Tiles.Count}), pos {nextPosition}, success: {isSuccess}");
    }

    private Vector2Int CalculateNextPosition(SnakeAction decision)
    {
        // Get the head's position and the current direction
        Vector2Int headPosition = Tiles[0].Position;
        Vector2Int currentDirection = Tiles.Count > 1
            ? Tiles[0].Position - Tiles[1].Position // Calculate direction from head to second tile
            : Vector2Int.up; // Default to upward movement for single-tile snakes

        // Adjust direction based on the decision
        Vector2Int nextDirection = decision switch
        {
            SnakeAction.MoveForward => currentDirection,
            SnakeAction.TurnLeft => new Vector2Int(-currentDirection.y, currentDirection.x), // Rotate left
            SnakeAction.TurnRight => new Vector2Int(currentDirection.y, -currentDirection.x), // Rotate right
            _ => throw new System.ArgumentException("Invalid SnakeAction provided.")
        };

        Debug.Log($"current dir {currentDirection} - head {headPosition} - moving {decision} - next pos {headPosition + nextDirection}");

        // Calculate the next position
        return headPosition + nextDirection;
    }
}
