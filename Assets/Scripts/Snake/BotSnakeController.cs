using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSnakeController : ISnakeController
{
    public SnakeAction MakeDecision(Snake snake)
    {
        return SnakeAction.MoveForward;
    }
}
