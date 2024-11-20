using UnityEngine;

public class BotSnakeController : ISnakeController
{
    readonly Snake _snake;
    readonly SnakeBoard _board;
    readonly int _depth;

    public BotSnakeController(Snake snake, SnakeBoard board, int depth)
    {
        _snake = snake;
        _board = board;
        _depth = depth;
    }

    public SnakeAction MakeDecision(Vector2Int headPosition, Vector2Int currentDirection)
    {
        // Possible actions
        var actions = new[] { SnakeAction.MoveForward, SnakeAction.TurnLeft, SnakeAction.TurnRight };

        // Evaluate each action
        SnakeAction bestAction = SnakeAction.MoveForward;
        int bestScore = int.MinValue;

        foreach (var action in actions)
        {
            // Calculate the next direction based on the action
            Vector2Int nextDirection = GetNextDirection(currentDirection, action);

            // Calculate the next position based on the next direction
            Vector2Int nextPosition = headPosition + nextDirection;

            // Skip invalid moves (out of bounds or occupied)
            if (!_board.IsPositionWithinBounds(nextPosition) || _board.IsPositionOccupied(nextPosition))
                continue;

            // Simulate the move and evaluate the score
            int score = SimulatePath(nextPosition, nextDirection, depth: _depth);

            if (score > bestScore)
            {
                bestScore = score;
                bestAction = action;
            }
        }

        return bestAction;
    }

    private Vector2Int GetNextDirection(Vector2Int currentDirection, SnakeAction action)
    {
        return action switch
        {
            SnakeAction.MoveForward => currentDirection,
            SnakeAction.TurnLeft => new Vector2Int(-currentDirection.y, currentDirection.x), // Rotate left
            SnakeAction.TurnRight => new Vector2Int(currentDirection.y, -currentDirection.x), // Rotate right
            _ => throw new System.ArgumentException("Invalid SnakeAction provided.")
        };
    }

    private int SimulatePath(Vector2Int position, Vector2Int direction, int depth)
    {
        if (depth == 0 || !_board.IsPositionWithinBounds(position) || _board.IsPositionOccupied(position))
            return 0; // End simulation if out of depth, bounds, or position is occupied

        int score = 1; // Base score for a valid move

        // Simulate next moves from this position
        foreach (var nextDirection in new[] { direction, new Vector2Int(-direction.y, direction.x), new Vector2Int(direction.y, -direction.x) })
        {
            Vector2Int nextPosition = position + nextDirection;

            // Avoid reversing direction
            if (nextDirection == -direction)
                continue;

            score += SimulatePath(nextPosition, nextDirection, depth - 1);
        }

        return score;
    }
}
