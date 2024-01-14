using System.Collections.Generic;
using System.Linq;
using Pathfinding.Scripts.Board;
using UnityEngine;

namespace Pathfinding.Scripts.Pathfinders
{
    /// <summary>
    /// Greedy Best pathfinding algorithm implementation.
    /// </summary>
    public class GreedyBest : Pathfinder
    {
        public GreedyBest(GameBoard gameBoard) : base(gameBoard) { }

        public override bool TryFindPath(Vector2Int start, Vector2Int end, out List<Vector2Int> path)
        {
            path = null;
            
            if (start == end)
                return false;
            
            Dictionary<Vector2Int, int> open = new Dictionary<Vector2Int, int> { { start, 0 } };
            Dictionary<Vector2Int, Vector2Int?> cameFrom = new Dictionary<Vector2Int, Vector2Int?> { {start, null}, };
            
            while (open.Count > 0)
            {
                Vector2Int current = open.Keys.FirstOrDefault();
                open.Remove(current);

                if (current == end)
                {
                    path = ReconstructPath(cameFrom, start, end);
                    return true;
                }

                foreach (Vector2Int next in gameBoard.GetPassableNeighbours(current))
                {
                    if (cameFrom.ContainsKey(next))
                        continue;

                    int distance = GetHeuristicDistance(end, next);
                    open.Add(next, distance);
                    cameFrom[next] = current;
                }
                
                open = open.OrderBy(entry => entry.Value).ToDictionary(entry => entry.Key, entry => entry.Value);
            }
            
            return false;
        }
    }
}