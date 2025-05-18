using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeOrchestrator : MonoBehaviourSingleton<SnakeOrchestrator>
{
    public List<Snake> Snakes { get; private set; } = new List<Snake>();

    private int _tickIntervalMilliseconds = 450;
    private int _startupHoldMilliseconds = 100;
    private bool _isRunning = true;

    private Queue<Snake> _enlistSnake = new Queue<Snake>();
    private Queue<Snake> _delistSnake = new Queue<Snake>();
    
    // Reference to player snake for game over detection
    private Snake _playerSnake;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        // Clear any existing snakes when the orchestrator is enabled
        Snakes.Clear();
        _enlistSnake.Clear();
        _delistSnake.Clear();
        _playerSnake = null;
    }

    private void Start()
    {
        StartCoroutine(TickLoop());
    }

    private IEnumerator TickLoop()
    {
        yield return new WaitForSeconds(_startupHoldMilliseconds / 1000f);

        while (_isRunning)
        {
            try
            {
                Tick();
            }
            catch (Exception ex) 
            {
                Debug.LogError($"Error in SnakeOrchestrator.Tick: {ex.Message}");
                break;
            }

            yield return new WaitForSeconds(_tickIntervalMilliseconds / 1000f);
        }
    }

    private void Tick()
    {
        while (_enlistSnake.Count > 0) {
            var item = _enlistSnake.Dequeue();
            Snakes.Add(item);
            
            // If this is the first snake (player's snake), store a reference
            if (Snakes.Count == 1)
            {
                _playerSnake = item;
            }
        }

        while (_delistSnake.Count > 0)
        {
            var item = _delistSnake.Dequeue();
            Snakes.Remove(item);
            
            // Check if the player snake was deregistered (player died)
            if (item == _playerSnake)
            {
                PlayerDied();
            }
        }

        foreach (var item in Snakes)
        {
            item.Move();
        }
    }
    
    private void PlayerDied()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.ShowGameOverScreen();
        }
        else
        {
            Debug.LogError("GameManager not found when trying to show game over screen");
        }
    }

    private void OnDestroy()
    {
        _isRunning = false;
        
        // Clear all references
        Snakes.Clear();
        _enlistSnake.Clear();
        _delistSnake.Clear();
        _playerSnake = null;
    }

    internal void Register(Snake snake)
    {
        if (Snakes.Contains(snake))
        {
            throw new Exception("Snake already registered");
        }

        _enlistSnake.Enqueue(snake);
    }

    internal void Deregister(Snake snake)
    {
        if (!Snakes.Contains(snake))
        {
            throw new Exception("Snake not registered");
        }

        _delistSnake.Enqueue(snake);
    }
}
