using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Snake))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerSnakeController : MonoBehaviour, ISnakeController
{
    // private PlayerInput _input;

    private SnakeAction _action = SnakeAction.MoveForward;

    public SnakeAction MakeDecision()
    {
        var buffer = _action;
        _action = SnakeAction.MoveForward;
        return buffer;
    }

    //private void Awake()
    //{
    //    _input = GetComponent<PlayerInput>();
    //    _input.onActionTriggered += Action;
    //}

    //private void Action(InputAction.CallbackContext context)
    //{
    //    context.ReadValue<Vector2>();
    //}

    public void InputMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();

        if (input == Vector2.up)
        {
            _action = SnakeAction.MoveForward;
        }
        else if (input == Vector2.left)
        {
            _action = SnakeAction.TurnLeft;
        }
        else if (input == Vector2.right)
        {
            _action = SnakeAction.TurnRight;
        }
    }

        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.W)) 
        //    {
        //        _action = SnakeAction.MoveForward;
        //    }
        //    else if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        _action = SnakeAction.TurnLeft;
        //    }
        //    else if (Input.GetKeyDown(KeyCode.D))
        //    {
        //        _action = SnakeAction.TurnRight;
        //    }
        //}
}
