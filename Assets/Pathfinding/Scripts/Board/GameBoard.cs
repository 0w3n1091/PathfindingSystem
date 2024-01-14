using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Pathfinding.Scripts.Board
{
    /// <summary>
    /// Represents the game board, manages grid positions, obstacles, and provides utility methods for pathfinding.
    /// </summary>
    public class GameBoard : MonoBehaviour
    {
        public List<Vector2Int> Grid { get; private set; } = new List<Vector2Int>();
        public event Action OnMapChanged; 

        [Inject] private readonly GameBoardConfig config;
        private readonly List<Vector2Int> obstacles = new List<Vector2Int>();
        
        /// <summary>
        /// Attempts to retrieve the tile position based on the provided world position.
        /// </summary>
        /// <param name="worldPosition">The world position for which to find the corresponding tile.</param>
        /// <param name="tilePosition">The output parameter containing the tile position if found, otherwise null.</param>
        /// <returns>True if a tile position is found, false otherwise.</returns>
        public bool TryGetTile(Vector3 worldPosition, out Vector2Int? tilePosition)
        {
            tilePosition = null;
            Vector2Int tileLocalPosition = WorldToGrid(worldPosition);
            
            foreach (Vector2Int tile in Grid)
            {
                if (tile != tileLocalPosition)
                    continue;

                tilePosition = tile;
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Attempts to retrieve the tile position under the cursor.
        /// </summary>
        /// <param name="tilePosition">The output parameter containing the tile position if found, otherwise null.</param>
        /// <returns>True if a tile position is found, false otherwise.</returns>
        public bool TryGetTileAtCursor(out Vector2Int? tilePosition)
        {
            tilePosition = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 cursorPosition = Raycast(ray);

            if (!TryGetTile(cursorPosition, out Vector2Int? result))
                return false;

            tilePosition = result;
            return true;
        }
        
        /// <summary>
        /// Retrieves a list of passable neighbors for a given target tile.
        /// </summary>
        /// <param name="target">The target tile for which to find passable neighbors.</param>
        /// <returns>A list of passable neighbors.</returns>
        public List<Vector2Int> GetPassableNeighbours(Vector2Int target)
        {
            List<Vector2Int> neighbours = new List<Vector2Int>();
        
            foreach (Vector2Int tile in Grid)
            {
                if (tile == target)
                    continue;

                if (Mathf.Abs(tile.x - target.x) <= config.TileSize && Mathf.Abs(tile.y - target.y) <= config.TileSize && IsPassable(target, tile))
                    neighbours.Add(tile);
            }
        
            return neighbours;
        }
        
        /// <summary>
        /// Converts a grid position to world space.
        /// </summary>
        /// <param name="gridPosition">The grid position to convert.</param>
        /// <returns>The corresponding world position.</returns>
        public Vector3 GridToWorld(Vector2Int gridPosition)
        {
            float halfTileSize = config.TileSize / 2f;
            float xPosition = transform.position.x - config.GridSize.x / 2f + gridPosition.x;
            float yPosition = transform.position.y - config.GridSize.y / 2f + gridPosition.y;
        
            xPosition += halfTileSize; 
            yPosition += halfTileSize; 
            
            return new Vector3(xPosition, 0f, yPosition);
        }
        
        /// <summary>
        /// Converts a world position to grid space.
        /// </summary>
        /// <param name="worldPosition">The world position to convert.</param>
        /// <returns>The corresponding grid position.</returns>
        public Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            Vector3 localPosition = transform.InverseTransformPoint(worldPosition);

            float halfWidth = (config.GridSize.x * config.TileSize) / 2f;
            float halfHeight = (config.GridSize.y * config.TileSize) / 2f;
            localPosition.x += halfWidth;
            localPosition.z += halfHeight;

            int gridX = Mathf.FloorToInt(localPosition.x / config.TileSize);
            int gridY = Mathf.FloorToInt(localPosition.z / config.TileSize);
            Vector2Int gridPosition = new Vector2Int(gridX, gridY);

            return gridPosition;
        }
        
        /// <summary>
        /// Converts a list of grid coordinates representing a path to a list of corresponding world positions.
        /// </summary>
        /// <param name="path">The list of grid coordinates representing a path.</param>
        /// <returns>A list of world positions corresponding to the input path.</returns>
        public List<Vector3> GridPathToWorldPath(List<Vector2Int> path)
        {
            List<Vector3> worldPath = new List<Vector3>();
            
            foreach (Vector2Int coord in path)
            {
                Vector3 worldPos = GridToWorld(coord);
                worldPos = new Vector3(worldPos.x, worldPos.y + transform.position.y, worldPos.z);
                worldPath.Add(worldPos);
            }

            return worldPath;
        }
        
        /// <summary>
        /// Adds an obstacle at the specified world position.
        /// </summary>
        /// <param name="worldPosition">The world position where the obstacle should be added.</param>
        public void AddObstacle(Vector3 worldPosition) 
        {
            Vector2Int gridPosition = WorldToGrid(worldPosition);
            
            if (obstacles.Contains(gridPosition))
                return;
            
            obstacles.Add(gridPosition);
            OnMapChanged?.Invoke();
        }
        
        /// <summary>
        /// Removes an obstacle at the specified world position.
        /// </summary>
        /// <param name="worldPosition">The world position where the obstacle should be removed.</param>
        public void RemoveObstacle(Vector3 worldPosition)
        {
            Vector2Int gridPosition = WorldToGrid(worldPosition);
            
            if (!obstacles.Contains(gridPosition))
                return;

            obstacles.Remove(gridPosition);
            OnMapChanged?.Invoke();
        }
        
        /// <summary>
        /// Checks if the specified grid position contains an obstacle.
        /// </summary>
        /// <param name="gridPosition">The grid position to check for obstacles.</param>
        /// <returns>True if the grid position contains an obstacle, false otherwise.</returns>
        public bool IsObstacle(Vector2Int gridPosition) => obstacles.Contains(gridPosition);
        
        #region Unity events
        
        private void Awake() => Initialize();
        
        #endregion
        
        #region Private methods
        
        /// <summary>
        /// Initializes game board.
        /// </summary>
        private void Initialize()
        {
            Grid = new List<Vector2Int>();
            
            for (int y = 0; y < config.GridSize.y; y++)
            {
                for (int x = 0; x < config.GridSize.x; x++)
                {
                    Vector2Int tile = new Vector2Int(x,y);
                    Grid.Add(tile);
                }
            }
            
            OnMapChanged?.Invoke();
        }
        
        /// <summary>
        /// Checks if the movement from one grid position to another is passable, considering obstacles and diagonal movement.
        /// </summary>
        /// <param name="from">The starting grid position.</param>
        /// <param name="to">The target grid position.</param>
        private bool IsPassable(Vector2Int from, Vector2Int to)
        {
            if (obstacles.Contains(to))
                return false;
            
            // If movement is diagonal 
            if (from.x != to.x && from.y != to.y)
            {
                if (!config.IsDiagonalMovementAllowed)
                    return false;
                
                Vector2Int horizontal = new Vector2Int(to.x, from.y);
                Vector2Int vertical = new Vector2Int(from.x, to.y);
        
                if (obstacles.Contains(horizontal) || obstacles.Contains(vertical))
                    return false;
            }
    
            return true;
        }
        
        /// <summary>
        /// Performs a raycast from the given ray and returns the intersection point with a plane defined by the transform's up direction.
        /// </summary>
        /// <param name="ray">The ray for the raycast.</param>
        /// <returns>The intersection point with the plane, or Vector3.zero if no intersection occurs.</returns>
        private Vector3 Raycast(Ray ray)
        {
            Vector3 point = new Vector3(0f, 0f, 0f);
            
            Plane plane = new Plane(transform.up, transform.position);
            if (plane.Raycast(ray, out var distance))
                point = ray.GetPoint(distance);
            
            return point;
        }
        
        #endregion
    }
}