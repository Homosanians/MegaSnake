using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISnakeController
{
    SnakeAction MakeDecision(Vector2Int headPosition, Vector2Int currentDirection);
}
