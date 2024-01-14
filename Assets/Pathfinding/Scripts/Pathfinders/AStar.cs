using System.Collections.Generic;
using System.Linq;
using Pathfinding.Scripts.Board;
using UnityEngine;

namespace Pathfinding.Scripts.Pathfinders
{
    /// <summary>
    /// A* pathfinding algorithm implementation.
    /// </summary>
    public class AStar : Pathfinder
    {
        public AStar(GameBoard gameBoard) : base(gameBoard) { }
        
        public override bool TryFindPath(Vector2Int start, Vector2Int end, out List<Vector2Int> path)
        {
            path = null;
            
            if (start == end)
                return false;
            
            List<Vector2Int> open = new List<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int?> cameFrom = new Dictionary<Vector2Int, Vector2Int?> { {start, null}, };

            Dictionary<Vector2Int, int> gCosts = new Dictionary<Vector2Int, int> { {start, 0} };
            Dictionary<Vector2Int, int> hCosts = new Dictionary<Vector2Int, int> { { start, 0 } };
            Dictionary<Vector2Int, int> fCosts = new Dictionary<Vector2Int, int> { { start, 0 } };

            open.Add(start);

            while (open.Count > 0)
            {
                Vector2Int current = fCosts.OrderBy(n => n.Value).ThenBy(n => hCosts[n.Key]).First().Key;

                if (current == end)
                {
                    path = ReconstructPath(cameFrom, start, end);
                    return true;
                }

                open.Remove(current);
                fCosts.Remove(current);

                foreach (Vector2Int next in gameBoard.GetPassableNeighbours(current))
                {
                    if (cameFrom.ContainsKey(next))
                        continue;
                    
                    int newCost = gCosts[current] + GetHeuristicDistance(end, next);

                    gCosts.TryAdd(next, 0);    
                    hCosts.TryAdd(next, 0);    
                    
                    if (newCost >= gCosts[next] && open.Contains(next))
                        continue;

                    gCosts[next] = newCost;
                    hCosts[next] = GetHeuristicDistance(end, next);
                    fCosts[next] = gCosts[next] + hCosts[next];
                    cameFrom[next] = current;

                    if (!open.Contains(next))
                        open.Add(next);
                }
            }
            
            return false;
        }
    }
}