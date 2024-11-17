using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Snake : MonoBehaviour
{
    public SnakeBoard Board { get; private set; }
    public List<SnakeTile> Tiles { get; private set; } = new List<SnakeTile>();
    private ISnakeController _controller = new BotSnakeController();
    public Tile tile;
    
    void Move()
    {
        var decision = _controller.MakeDecision(this);
        
    }

    public void Initialize(SnakeBoard snakeBoard, Vector2Int position, int length)
    {
        Board = snakeBoard;
    }
}
