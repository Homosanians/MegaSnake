using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoard : MonoBehaviour
{
    public List<Snake> Snakes { get; private set; } = new List<Snake>();

    public bool IsPositionValid()
    {
        return false;
    }
}
