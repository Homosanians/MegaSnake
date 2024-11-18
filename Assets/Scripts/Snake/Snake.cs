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
        var headTile = new SnakeTile(order, letter);
        headTile.Position = position;
        Tiles.Add(headTile);
        Board.SetTile(position.ToVector3Int(), headTile.Letter);
    }

    public void Initialize(SnakeBoard snakeBoard, Vector2Int position, int length)
    {
        Board = snakeBoard;
        _controller = new BotSnakeController(this, Board);

        SnakeOrchestrator.Instance.Register(this);

        // Create and set the head tile
        AddSnakeTile(0, position, "H");

        Vector2Int currentPosition = position;

        // Create and set the body tiles
        for (int i = 1; i < length; i++)
        {
            Vector3Int? nextPosition = Board.FindFreeAdjacentPosition(currentPosition.ToVector3Int(), prefferedDirection: new Vector3Int(0, -1, 0));
            if (nextPosition.HasValue)
            {
                AddSnakeTile(i, nextPosition.Value.ToVector2Int(), "B");
                currentPosition = nextPosition.Value.ToVector2Int();
            }
            else
            {
                throw new System.Exception("Snake can't extend further at this point.");
            }
        }
    }

    public void Move()
    {
        Debug.Log("Move");
        foreach (var item in Tiles)
        {
            Debug.Log(item.Position);
        }

        var decision = _controller.MakeDecision();

        // Calculate the next position based on the decision
        Vector2Int nextPosition = CalculateNextPosition(decision);

        // Pass the next position to the board to attempt movement
        bool isSuccess = Board.TryMoveSnake(this, nextPosition);

        Debug.Log($"Moving snake {decision} (count {Tiles.Count}), pos {nextPosition}, success: {isSuccess}");
    }

    private Vector2Int CalculateNextPosition(SnakeAction decision)
    {
        Vector2Int currentDirection = Tiles.Count > 1
            ? Tiles.Find(x => x.Order == 0).Position - Tiles.Find(x => x.Order == 1).Position // Head direction
            : Vector2Int.up; // Default to upward movement for single-tile snakes

        return decision switch
        {
            SnakeAction.MoveForward => Tiles.Find(x => x.Order == 0).Position + currentDirection,
            SnakeAction.TurnLeft => Tiles.Find(x => x.Order == 0).Position + new Vector2Int(-1, 0),
            SnakeAction.TurnRight => Tiles.Find(x => x.Order == 0).Position + new Vector2Int(1, 0),
            _ => throw new System.ArgumentException("Invalid SnakeAction provided.")
        };
    }
}
