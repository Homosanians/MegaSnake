using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SnakeTile : TileBase
{
    public Snake? Parent { get; private set; }
    public string Letter { get; private set; }
    public SnakeTailState SnakeTailState { get; private set; }
    public Vector2Int Position {get;set;}
    public int Order { get; set; }

    public SnakeTile(int order, string letter, SnakeTailState snakeTailState = SnakeTailState.PartOfLivingSnake)
    {
        Order = order;
        Letter = letter;
        SnakeTailState = snakeTailState;
    }
}
