using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSnakeController : ISnakeController
{
    readonly Snake _snake;
    readonly SnakeBoard _board;

    public BotSnakeController(Snake snake, SnakeBoard board)
    {
        _snake = snake;
        _board = board;
    }

    public SnakeAction MakeDecision()
    {
        return SnakeAction.MoveForward;
    }
}
