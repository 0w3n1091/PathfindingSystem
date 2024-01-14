using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Pathfinding.Scripts.Board
{
    /// <summary>
    /// Handles drawing Gizmos for visualizing the game board, paths, start and end points during runtime.
    /// </summary>
    public class GizmosDrawer : MonoBehaviour
    {
        [Header("References")] 
        [Inject] private PathfinderDemo pathfinderDemo;
        [Inject] private GameBoard gameBoard;
        [Inject] private GameBoardConfig config;

        [Header("Colors")] 
        [SerializeField] private Color gridColor = Color.red;
        [SerializeField] private Color pathColor = Color.green;
        [SerializeField] private Color startColor = Color.blue;
        [SerializeField] private Color endColor = Color.yellow;

        private List<Vector3> _route;
        private Vector3? _startPosition;
        private Vector3? _endPosition;

        /// <summary>
        /// Handles the event when the start tile is changed.
        /// </summary>
        /// <param name="start">The new start tile position, or null if there is no start tile.</param>
        private void OnStartTileChanged(Vector2Int? start) => _startPosition = start != null ? gameBoard.GridToWorld(start.Value) : null;
        
        /// <summary>
        /// Handles the event when the end tile is changed.
        /// </summary>
        /// <param name="end">The new end tile position, or null if there is no end tile.</param>
        private void OnEndTileChanged(Vector2Int? end) => _endPosition = end != null ? gameBoard.GridToWorld(end.Value) : null;
        
        /// <summary>
        /// Handles the event when the route is changed.
        /// </summary>
        /// <param name="route">The new route represented as a list of grid coordinates.</param>
        private void OnRouteChanged(List<Vector2Int> route)
        {
            if (route == null)
            {
                _route = null;
                return;
            }
            
            _route = new List<Vector3>();

            foreach (Vector2Int tile in route)
            {
                Vector3 position = gameBoard.GridToWorld(tile);
                _route.Add(position);
            }
        }
        
        /// <summary>
        /// Draws the grid using Gizmos in the scene.
        /// </summary>
        private void DrawGrid()
        {
            Gizmos.color = gridColor;

            foreach (Vector2Int tile in gameBoard.Grid)
            {
                Vector3 worldPos = gameBoard.GridToWorld(tile);
                Vector3 size = new Vector3(config.TileSize, 0f, config.TileSize);
                
                Gizmos.DrawWireCube(worldPos, size);
            }
        }
        
        /// <summary>
        /// Draws a path using Gizmos in the scene.
        /// </summary>
        private void DrawPath()
        {
            if (_route == null)
                return;
            
            for (int i = 0; i < _route.Count; i++)
            {
                if (i == _route.Count - 1)
                    return;

                Vector3 start = _route[i];
                Vector3 end = _route[i + 1];

                Handles.DrawBezier(start, end, start, end, pathColor, null, 2);
            }
        }
        
        /// <summary>
        /// Draws the start point using Gizmos in the scene.
        /// </summary>
        private void DrawStart()
        {
            if (_startPosition == null)
                return;
            
            Gizmos.color = startColor;
            DrawSphere(_startPosition.Value);
        }
        
        /// <summary>
        /// Draws the end point using Gizmos in the scene.
        /// </summary>
        private void DrawEnd()
        {
            if (_endPosition == null)
                return;
            
            Gizmos.color = endColor;
            DrawSphere(_endPosition.Value);
        }
        
        /// <summary>
        /// Draws a sphere at the specified position using Gizmos in the scene.
        /// </summary>
        /// <param name="position">The position at which to draw the sphere.</param>
        private void DrawSphere(Vector3 position)
        {
            float radius = config.TileSize / 8f;
            Gizmos.DrawSphere(position, radius);
        }
        
        #region Unity events
        
        private void OnEnable()
        {
            pathfinderDemo.StartTile.Subscribe(OnStartTileChanged);
            pathfinderDemo.EndTile.Subscribe(OnEndTileChanged);
            pathfinderDemo.Route.Subscribe(OnRouteChanged);
        }

        private void OnDisable()
        {
            pathfinderDemo.StartTile.Dispose(OnStartTileChanged);
            pathfinderDemo.EndTile.Dispose(OnEndTileChanged);
            pathfinderDemo.Route.Dispose(OnRouteChanged);
        }
        
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            if (config.DrawGrid)
                DrawGrid();
            
            DrawPath();
            DrawStart();
            DrawEnd();
        }
        
        #endregion
    }
}