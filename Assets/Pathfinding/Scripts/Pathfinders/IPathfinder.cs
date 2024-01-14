using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Scripts.Pathfinders
{
    /// <summary>
    /// Interface for pathfinding algorithms.
    /// </summary>
    public interface IPathfinder
    {
        /// <summary>
        /// <param name="start">The starting position on the grid.</param>
        /// <param name="end">The target position on the grid.</param>
        /// <param name="route">The calculated path as a list of grid positions if successful; otherwise, null.</param>
        /// <returns>True if a path was successfully found; otherwise, false.</returns>
        /// </summary>
        public bool TryFindPath(Vector2Int start, Vector2Int end, out List<Vector2Int> route);
    }
}