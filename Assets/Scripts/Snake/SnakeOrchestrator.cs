using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeOrchestrator : MonoBehaviour
{
    public static SnakeOrchestrator Instance { get; private set; }

    public List<Snake> Snakes { get; private set; } = new List<Snake>();

    [SerializeField] private int _tickIntervalMilliseconds = 500;
    [SerializeField] private int _startupHoldMilliseconds = 500;
    private bool _isRunning = true;

    private void Awake()
    {
        if (Instance != null)
        {
            throw new System.Exception("Singleton duplicate");
        }

        Instance = this;
    }

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

        Snakes.Add(snake);
    }
}
