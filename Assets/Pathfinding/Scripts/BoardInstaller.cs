using Pathfinding.Scripts.Board;
using UnityEngine;
using Zenject;

namespace Pathfinding.Scripts
{
    /// <summary>
    /// Installer for setting up Zenject bindings related to the game board.
    /// </summary>
    public class BoardInstaller : MonoInstaller
    {
        [SerializeField] private GameBoard gameBoard;
        [SerializeField] private PathfinderDemo pathfinderDemo;
        [SerializeField] private GameBoardConfig gameBoardConfig;

        /// <summary>
        /// Installs Zenject bindings for the game board-related components.
        /// </summary>
        public override void InstallBindings() => BindBoard();

        /// <summary>
        /// Binds instances of the game board, pathfinder demo, and game board config to their respective types.
        /// </summary>
        private void BindBoard()
        {
            Container.Bind<GameBoard>().FromInstance(gameBoard).AsSingle().NonLazy();
            Container.Bind<PathfinderDemo>().FromInstance(pathfinderDemo).AsSingle().NonLazy();
            Container.Bind<GameBoardConfig>().FromInstance(gameBoardConfig).AsSingle().NonLazy();
        }
    }
}