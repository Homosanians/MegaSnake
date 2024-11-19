using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SnakeOrchestrator : MonoBehaviourSingleton<SnakeOrchestrator>
{
    public List<Snake> Snakes { get; private set; } = new List<Snake>();

    private int _tickIntervalMilliseconds = 100;
    private int _startupHoldMilliseconds = 0;
    private bool _isRunning = true;

    private Queue<Snake> _enlistSnake = new Queue<Snake>();
    private Queue<Snake> _delistSnake = new Queue<Snake>();

    private void Start()
    {
        StartCoroutine(TickLoop());
    }

    private IEnumerator TickLoop()
    {
        yield return new WaitForSeconds(_tickIntervalMilliseconds / 1000f);

        while (_isRunning)
        {
            Tick();

            yield return new WaitForSeconds(_tickIntervalMilliseconds / 1000f);
        }
    }

    private void Tick()
    {
        while (_enlistSnake.Count > 0) {
            var item = _enlistSnake.Dequeue();
            Snakes.Add(item);
        }

        while (_delistSnake.Count > 0)
        {
            var item = _delistSnake.Dequeue();
            Snakes.Remove(item);
        }

        foreach (var item in Snakes)
        {
            item.Move();
        }

        Debug.Log("Tick called at: " + Time.time);
    }

    private void OnDestroy()
    {
        _isRunning = false;
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
