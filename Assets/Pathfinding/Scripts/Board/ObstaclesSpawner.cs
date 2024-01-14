using UnityEngine;
using Zenject;

namespace Pathfinding.Scripts.Board
{
    /// <summary>
    /// Spawns obstacles in the game world based on user input.
    /// </summary>
    public class ObstaclesSpawner : MonoBehaviour
    {
        [Inject] private GameBoard gameBoard;
        [SerializeField] private GameObject obstaclePrefab;

        private void Update()
        {
            if (Input.GetMouseButtonDown(2))
                TryInstantiate();
        }
        
        /// <summary>
        /// Attempts to instantiate an obstacle at the cursor's position on the game board.
        /// </summary>
        private void TryInstantiate()
        {
            if (!gameBoard.TryGetTileAtCursor(out Vector2Int? gridPosition))
                return;

            if (gameBoard.IsObstacle(gridPosition.Value))
                return;

            Vector3 worldPosition = gameBoard.GridToWorld(gridPosition.Value);
            Instantiate(obstaclePrefab, worldPosition, Quaternion.identity);
            gameBoard.AddObstacle(worldPosition);
        }
    }
}