using System.Collections.Generic;
using System.Linq;
using Pathfinding.Scripts.Board;
using UnityEngine;

namespace Pathfinding.Scripts.Pathfinders
{
    /// <summary>
    /// Breadth First pathfinding algorithm implementation.
    /// </summary>
    public class BreadthFirst : Pathfinder
    {
        public BreadthFirst(GameBoard gameBoard) : base(gameBoard) { }
        
        public override bool TryFindPath(Vector2Int start, Vector2Int end, out List<Vector2Int> path)
        {
            path = null;
            
            if (start == end)
                return false;
            
            List<Vector2Int> open = new List<Vector2Int> { start };
            Dictionary<Vector2Int, Vector2Int?> cameFrom = new Dictionary<Vector2Int, Vector2Int?> { {start, null}, };
            
            while (open.Count > 0)
            {
                Vector2Int current = open.FirstOrDefault();
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

                    open.Add(next);
                    cameFrom[next] = current;
                }
            }
            
            return false;
        }
    }
}