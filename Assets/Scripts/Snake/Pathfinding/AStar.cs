using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private SnakeBoard _board;
    private List<Vector2Int> _directions = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    public AStar(SnakeBoard board)
    {
        _board = board;
    }

    // Find path to the position furthest from target
    public Vector2Int FindFurthestPoint(Vector2Int startPos, Vector2Int targetPos, int maxDepth = 10)
    {
        Dictionary<Vector2Int, float> distances = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        
        // Initialize with start position
        PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();
        frontier.Enqueue(startPos, 0);
        
        distances[startPos] = 0;
        
        Vector2Int furthestPoint = startPos;
        float maxDistance = 0;
        
        while (frontier.Count > 0 && distances[furthestPoint] < maxDepth)
        {
            Vector2Int current = frontier.Dequeue();
            
            // Check if this is the furthest point so far
            float distanceToTarget = Vector2Int.Distance(current, targetPos);
            if (distanceToTarget > maxDistance)
            {
                maxDistance = distanceToTarget;
                furthestPoint = current;
            }
            
            // Explore neighbors
            foreach (Vector2Int direction in _directions)
            {
                Vector2Int neighbor = current + direction;
                
                // Skip if out of bounds or occupied
                if (!_board.IsPositionWithinBounds(neighbor) || _board.IsPositionOccupied(neighbor))
                {
                    continue;
                }
                
                float newDistance = distances[current] + 1;
                
                // If we haven't explored this node or found a better path
                if (!distances.ContainsKey(neighbor) || newDistance < distances[neighbor])
                {
                    distances[neighbor] = newDistance;
                    
                    // Priority is negative distance to target (to find furthest point)
                    float priority = newDistance + Vector2Int.Distance(neighbor, targetPos) * -1;
                    frontier.Enqueue(neighbor, priority);
                    
                    cameFrom[neighbor] = current;
                }
            }
        }
        
        // Now we have the furthest reachable point, reconstruct the first step to take
        if (furthestPoint != startPos)
        {
            Vector2Int current = furthestPoint;
            while (cameFrom[current] != startPos)
            {
                current = cameFrom[current];
            }
            return current;
        }
        
        // If no path found, return a random valid direction
        foreach (Vector2Int direction in _directions)
        {
            Vector2Int neighbor = startPos + direction;
            if (_board.IsPositionWithinBounds(neighbor) && !_board.IsPositionOccupied(neighbor))
            {
                return neighbor;
            }
        }
        
        // If still no valid move, return the start position (will likely result in death)
        return startPos;
    }
    
    // Simple priority queue implementation for A* algorithm
    private class PriorityQueue<T>
    {
        private List<KeyValuePair<T, float>> _elements = new List<KeyValuePair<T, float>>();
        
        public int Count => _elements.Count;
        
        public void Enqueue(T item, float priority)
        {
            _elements.Add(new KeyValuePair<T, float>(item, priority));
        }
        
        public T Dequeue()
        {
            int bestIndex = 0;
            
            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i].Value < _elements[bestIndex].Value)
                {
                    bestIndex = i;
                }
            }
            
            T bestItem = _elements[bestIndex].Key;
            _elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }
} 