using System.Collections.Generic;
using Pathfinding.Scripts.Board;
using UnityEngine;

namespace Pathfinding.Scripts.Pathfinders
{
    /// <summary>
    /// An abstract base class for pathfinding algorithms implementing the IPathfinder interface.
    /// </summary>
    public abstract class Pathfinder : IPathfinder
    {
        protected GameBoard gameBoard;
        
        protected Pathfinder(GameBoard gameBoard) => this.gameBoard = gameBoard;

        public abstract bool TryFindPath(Vector2Int start, Vector2Int end, out List<Vector2Int> route);
        
        /// <summary>
        /// Reconstructs and returns a path from the end to the start based on a cameFrom dictionary.
        /// </summary>
        /// <param name="cameFrom">A dictionary containing the relationship between grid positions in the path.</param>
        /// <param name="start">The starting position of the path.</param>
        /// <param name="end">The end position of the path.</param>
        protected List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int?> cameFrom, Vector2Int start, Vector2Int end)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Vector2Int current = end;
        
            while (current != start)
            {
                path.Add(current);
                current = cameFrom[current].Value;
            }
        
            path.Add(start); 
            path.Reverse();
            
            return path;
        }
        
        /// <summary>
        /// Calculates and returns the heuristic distance (Manhattan distance) between two grid positions.
        /// </summary>
        /// <param name="a">The first grid position.</param>
        /// <param name="b">The second grid position.</param>
        /// <returns>The calculated heuristic distance.</returns
        protected int GetHeuristicDistance(Vector2Int a, Vector2Int b)
        {
            int distX = Mathf.Abs(a.x - b.x);
            int distY = Mathf.Abs(a.y - b.y);

            return distX+distY; 
        }
    }
}