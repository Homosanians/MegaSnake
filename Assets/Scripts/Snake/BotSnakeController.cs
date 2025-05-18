using UnityEngine;

public class BotSnakeController : ISnakeController
{
    private readonly Snake _snake;
    private readonly SnakeBoard _board;
    private readonly int _depth;
    private readonly AStar _pathfinder;

    public BotSnakeController(Snake snake, SnakeBoard board, int depth)
    {
        _snake = snake;
        _board = board;
        _depth = depth;
        _pathfinder = new AStar(board);
    }

    public SnakeAction MakeDecision(Vector2Int headPosition, Vector2Int currentDirection)
    {
        // Find the player snake
        Snake playerSnake = null;
        foreach (var snake in SnakeOrchestrator.Instance.Snakes)
        {
            if (snake != _snake)
            {
                playerSnake = snake;
                break;
            }
        }

        if (playerSnake == null || playerSnake.Tiles.Count == 0)
        {
            // No player snake found or the player has no tiles, just use the old behavior
            return FallbackDecision(headPosition, currentDirection);
        }

        // Get player snake head position
        Vector2Int playerHeadPosition = playerSnake.Tiles[0].Position;

        // Use A* to find the best position to move to (furthest from player)
        Vector2Int bestNextPosition = _pathfinder.FindFurthestPoint(headPosition, playerHeadPosition, _depth);

        // Convert the position to an action
        Vector2Int moveVector = bestNextPosition - headPosition;
        
        // If the best next position is the current position or invalid, use fallback
        if (moveVector == Vector2Int.zero || !_board.IsPositionWithinBounds(bestNextPosition) || _board.IsPositionOccupied(bestNextPosition))
        {
            return FallbackDecision(headPosition, currentDirection);
        }

        // Convert the move vector to an action
        if (moveVector == currentDirection)
        {
            return SnakeAction.MoveForward;
        }
        else if (moveVector == new Vector2Int(-currentDirection.y, currentDirection.x)) // Left turn
        {
            return SnakeAction.TurnLeft;
        }
        else if (moveVector == new Vector2Int(currentDirection.y, -currentDirection.x)) // Right turn
        {
            return SnakeAction.TurnRight;
        }
        
        // If we can't determine a valid action, use fallback
        return FallbackDecision(headPosition, currentDirection);
    }

    private SnakeAction FallbackDecision(Vector2Int headPosition, Vector2Int currentDirection)
    {
        // Original logic as fallback
        var actions = new[] { SnakeAction.MoveForward, SnakeAction.TurnLeft, SnakeAction.TurnRight };

        SnakeAction bestAction = SnakeAction.MoveForward;
        int bestScore = int.MinValue;

        foreach (var action in actions)
        {
            Vector2Int nextDirection = GetNextDirection(currentDirection, action);
            Vector2Int nextPosition = headPosition + nextDirection;

            var nextPositionData = _board.GetPositionData(nextPosition);
            bool isNextPositionLivingSnakeHead = nextPositionData.IsTileSnakeHead.HasValue && 
                                                 nextPositionData.SnakeTile.SnakeTailState == SnakeTileState.PartOfLivingSnake && 
                                                 nextPositionData.IsTileSnakeHead.Value;

            if (!_board.IsPositionWithinBounds(nextPosition) || isNextPositionLivingSnakeHead)
                continue;

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
        var positionData = _board.GetPositionData(position);
        bool isPositionLivingSnakeHead = positionData.IsTileSnakeHead.HasValue && 
                                         positionData.SnakeTile.SnakeTailState == SnakeTileState.PartOfLivingSnake && 
                                         positionData.IsTileSnakeHead.Value;

        if (depth == 0 || !_board.IsPositionWithinBounds(position) || isPositionLivingSnakeHead)
            return 0;

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
