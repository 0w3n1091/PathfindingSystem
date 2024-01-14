using Pathfinding.Scripts.Board;
using Pathfinding.Scripts.Pathfinders;

namespace Pathfinding.Scripts
{
    /// <summary>
    /// Provides instances of pathfinding algorithms based on the specified search algorithm in the configuration.
    /// </summary>
    public class PathfindersProvider
    {
        private readonly GameBoard gameBoard;
        private readonly GameBoardConfig config;

        /// <summary>
        /// Constructs a PathfindersProvider with references to the GameBoard and GameBoardConfig.
        /// </summary>
        /// <param name="gameBoard">The GameBoard instance used for pathfinding.</param>
        /// <param name="config">The GameBoardConfig specifying the search algorithm.</param>
        public PathfindersProvider(GameBoard gameBoard, GameBoardConfig config)
        {
            this.gameBoard = gameBoard;
            this.config = config;
        }
        
        /// <summary>
        /// Gets an instance of the specified pathfinding algorithm based on the configured search algorithm.
        /// </summary>
        /// <returns>An instance of the selected pathfinding algorithm or null if the algorithm is not recognized.</returns>
        public IPathfinder Get()
        {
            return config.SearchAlgorithm switch
            {
                SearchAlgorithm.BreadthFirst => new BreadthFirst(gameBoard),
                SearchAlgorithm.GreedyBest => new GreedyBest(gameBoard),
                SearchAlgorithm.AStar => new AStar(gameBoard),
                _ => null
            };
        }
    }
}