using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class SnakeTile
{
    public SnakeBoard SnakeBoard { get; }
    public Snake? Parent { get; private set; }
    public string Letter { get; private set; }
    public SnakeTileState SnakeTailState { get; private set; }
    public Vector2Int Position { get; set; }
    public int Order { get; set; }
    public UnityEngine.Tilemaps.Tile CustomTile { get; private set; }

    public SnakeTile(SnakeBoard snakeBoard, Snake snake, int order, Vector2Int position, string letter, UnityEngine.Tilemaps.Tile tile, SnakeTileState snakeTailState = SnakeTileState.PartOfLivingSnake)
    {
        SnakeBoard = snakeBoard;
        Parent = snake;
        Order = order;
        Position = position;
        Letter = letter;
        SnakeTailState = snakeTailState;

        // Clone the tile to ensure unique text for each tile
        CustomTile = ScriptableObject.Instantiate(tile);
        CustomTile.name = $"Tile_{letter}";

        UpdateText(letter);
    }

    public void ChangeState(SnakeTileState newSnakeTailState)
    {
        if (newSnakeTailState == SnakeTileState.PartOfLivingSnake)
        {
            SnakeBoard.SetColor(CustomTile, Position, Color.red);
            SnakeTailState = newSnakeTailState;
        }
        else if (newSnakeTailState == SnakeTileState.Orphaned)
        {
            SnakeBoard.SetColor(CustomTile, Position, Color.gray);
            SnakeTailState = newSnakeTailState;
        }
    }

    public void UpdateText(string newText)
    {
        Letter = newText;
        var textComponent = CustomTile.gameObject.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.text = newText;
        }
        else
        {
            Debug.LogError("TMP_Text component not found on the tile.");
        }
    }

    internal void Hit(SnakeTile hitBySnakeTile)
    {
        if (Parent != null)
            Parent.Die();
    }
}
