using System.Collections.Generic;
using Pathfinding.Scripts.Board;
using Pathfinding.Scripts.Pathfinders;
using Pathfinding.Scripts.ReactiveProperty;
using UnityEngine;
using Zenject;

namespace Pathfinding.Scripts
{
    /// <summary>
    /// Main class for the pathfinding demo.
    /// </summary>
    public class PathfinderDemo : MonoBehaviour
    {
        public IReadonlyReactiveProperty<Vector2Int?> StartTile => _startTile;
        public IReadonlyReactiveProperty<Vector2Int?> EndTile => _endTile;
        public IReadonlyReactiveProperty<List<Vector2Int>> Route => _route;

        private readonly ReactiveProperty<Vector2Int?> _startTile = new ReactiveProperty<Vector2Int?>();
        private readonly ReactiveProperty<Vector2Int?> _endTile = new ReactiveProperty<Vector2Int?>();
        private readonly ReactiveProperty<List<Vector2Int>> _route = new ReactiveProperty<List<Vector2Int>>();
    
        private PathfindersProvider _pathfindersProvider;
        private GameBoard _gameBoard;
        private GameBoardConfig _config;
    
        [Inject]
        private void Construct(GameBoard gameBoard, GameBoardConfig config)
        {
            _gameBoard = gameBoard;
            _config = config;

            _pathfindersProvider = new PathfindersProvider(gameBoard, config);
            gameBoard.OnMapChanged += OnMapChanged;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_gameBoard.TryGetTileAtCursor(out Vector2Int? tile))
                    return;
            
                _startTile.Set(tile);;
            
                if (_startTile.Property != null && _endTile.Property != null) 
                    FindPath();
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (!_gameBoard.TryGetTileAtCursor(out Vector2Int? tile))
                    return;

                _endTile.Set(tile);
                
                if (_startTile.Property != null && _endTile.Property != null)
                    FindPath();
            }
        }
    
        private void FindPath()
        {
            IPathfinder pathfinder = _pathfindersProvider.Get();
            if (pathfinder.TryFindPath(_startTile.Property.Value, _endTile.Property.Value, out List<Vector2Int> route))
            {
                _route.Set(route);
                Debug.Log($"Path has been found using [{_config.SearchAlgorithm}] algorithm.");
                return;
            }

            _route.Set(null);
            Debug.LogWarning($"Path could not be found while using [{_config.SearchAlgorithm}] algorithm.");
        }

        private void OnMapChanged()
        {
            if (_route.Property != null)
                FindPath();
        }

        private void OnDisable() => _gameBoard.OnMapChanged -= OnMapChanged;
    }
}