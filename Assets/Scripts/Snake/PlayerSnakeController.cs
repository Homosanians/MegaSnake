using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput))]
public class PlayerSnakeController : MonoBehaviour, ISnakeController
{
    private Vector2 _inputDirection;

    [SerializeField]
    private bool _absoluteMovement = false;
    
    // Button input state
    private SnakeAction _pendingAction = SnakeAction.MoveForward;

    public SnakeAction MakeDecision(Vector2Int headPosition, Vector2Int currentDirection)
    {
        // If using button input, use the pending action
        if (_pendingAction != SnakeAction.MoveForward)
        {
            SnakeAction action = _pendingAction;
            _pendingAction = SnakeAction.MoveForward; // Reset after using
            return action;
        }
    
        // Input system input handling
        // Gamepad, joystick require this
        _inputDirection = _inputDirection.normalized;

        if (_absoluteMovement)
        {
            Vector2Int inputDirectionInt = Vector2Int.zero;

            if (_inputDirection == Vector2.up)
                inputDirectionInt = Vector2Int.up;
            else if (_inputDirection == Vector2.down)
                inputDirectionInt = Vector2Int.down;
            else if (_inputDirection == Vector2.left)
                inputDirectionInt = Vector2Int.left;
            else if (_inputDirection == Vector2.right)
                inputDirectionInt = Vector2Int.right;

            // Check if the input direction is valid and not opposite to the current direction
            if (inputDirectionInt != Vector2Int.zero && inputDirectionInt != -currentDirection)
            {
                if (inputDirectionInt == currentDirection)
                {
                    return SnakeAction.MoveForward;
                }
                else if (IsTurnLeft(currentDirection, inputDirectionInt))
                {
                    return SnakeAction.TurnLeft;
                }
                else if (IsTurnRight(currentDirection, inputDirectionInt))
                {
                    return SnakeAction.TurnRight;
                }
            }

            // Fallback to moving forward if no valid input or invalid turn
            return SnakeAction.MoveForward;
        }
        else
        {
            if (_inputDirection == Vector2.up)
            {
                _inputDirection = Vector2.up;
                return SnakeAction.MoveForward;
            }
            else if (_inputDirection == Vector2.left)
            {
                _inputDirection = Vector2.up;
                return SnakeAction.TurnLeft;
            }
            else if (_inputDirection == Vector2.right)
            {
                _inputDirection = Vector2.up;
                return SnakeAction.TurnRight;
            }
            else if (_inputDirection == Vector2.down)
            {
                _inputDirection = Vector2.up;
                return SnakeAction.MoveForward;
            }
        }

        Debug.LogError("Unexpected execution of player snake controller. Cant make relevant action decision. Moving forward.");
        return SnakeAction.MoveForward;
    }

    public void InputMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        _inputDirection = input;
    }
    
    // Button input methods for UI buttons
    public void InputButtonLeft()
    {
        _pendingAction = SnakeAction.TurnLeft;
    }
    
    public void InputButtonRight()
    {
        _pendingAction = SnakeAction.TurnRight;
    }

    // Helper method to determine if the input is a left turn relative to the current direction
    private bool IsTurnLeft(Vector2Int currentDirection, Vector2Int inputDirection)
    {
        return (currentDirection == Vector2Int.up && inputDirection == Vector2Int.left) ||
               (currentDirection == Vector2Int.left && inputDirection == Vector2Int.down) ||
               (currentDirection == Vector2Int.down && inputDirection == Vector2Int.right) ||
               (currentDirection == Vector2Int.right && inputDirection == Vector2Int.up);
    }

    // Helper method to determine if the input is a right turn relative to the current direction
    private bool IsTurnRight(Vector2Int currentDirection, Vector2Int inputDirection)
    {
        return (currentDirection == Vector2Int.up && inputDirection == Vector2Int.right) ||
               (currentDirection == Vector2Int.right && inputDirection == Vector2Int.down) ||
               (currentDirection == Vector2Int.down && inputDirection == Vector2Int.left) ||
               (currentDirection == Vector2Int.left && inputDirection == Vector2Int.up);
    }
}
