using UnityEngine;

namespace Pathfinding.Scripts.Board
{
    /// <summary>
    /// Configuration data for the GameBoard, specifying tile size, search algorithm, grid size, and related settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/GameBoardConfig", fileName = "GameBoardConfig", order = 0)]
    public class GameBoardConfig : ScriptableObject
    {
        public int TileSize { get; private set; } = 1;
        [field: SerializeField] public SearchAlgorithm SearchAlgorithm { get; private set; }
        [field: SerializeField] public Vector2Int GridSize { get; private set; }
        [field: SerializeField] public bool DrawGrid { get; private set; }
        [field: SerializeField] public bool IsDiagonalMovementAllowed { get; private set; }
    }
}