using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class SnakeTile
{
    public Snake? Parent { get; private set; }
    public string Letter { get; private set; }
    public SnakeTailState SnakeTailState { get; private set; }
    public Vector2Int Position {get;set;}
    public int Order { get; set; }
    public UnityEngine.Tilemaps.Tile CustomTile { get; private set; }

    public SnakeTile(int order, Vector2Int position, string letter, UnityEngine.Tilemaps.Tile tile, SnakeTailState snakeTailState = SnakeTailState.PartOfLivingSnake)
    {
        Order = order;
        Position = position;
        Letter = letter;
        SnakeTailState = snakeTailState;

        // Clone the tile to ensure unique text for each tile
        CustomTile = ScriptableObject.Instantiate(tile);
        CustomTile.name = $"Tile_{letter}";

        UpdateText(letter);
    }

    public void ChangeState(SnakeTailState snakeTailState)
    {
        if (SnakeTailState == SnakeTailState.PartOfLivingSnake)
        {
            CustomTile.color = Color.white;
            SnakeTailState = snakeTailState;
        }
        else if (SnakeTailState == SnakeTailState.Orphaned)
        {
            CustomTile.color = Color.gray;
            SnakeTailState = snakeTailState;
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

    public void SetLetter(string letter)
    {
        Letter = letter;
        var textComponent = CustomTile.gameObject.GetComponent<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.text = letter;
        }
    }
}
