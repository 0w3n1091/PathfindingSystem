using UnityEngine;
using Zenject;

namespace Pathfinding.Scripts.Board
{
    /// <summary>
    /// Represents an obstacle in the game world. Automatically adds itself to the GameBoard's obstacle list upon instantiation.
    /// </summary>
    public class Obstacle : MonoBehaviour
    {
        [Inject]
        private void Construct(GameBoard gameBoard)
        {
            gameBoard.AddObstacle(transform.position);
        }
    }
}