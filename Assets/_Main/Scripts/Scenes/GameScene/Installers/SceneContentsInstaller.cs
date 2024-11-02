using _Main.Scripts.Scenes.GameScene.Gameplay.GameBoard.View;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Installers
{
    public class SceneContentsInstaller : MonoInstaller
    {
        [SerializeField] private GameBoardContent gameBoardContent;
        
        public override void InstallBindings()
        {
            Container.Bind<GameBoardContent>().FromInstance(gameBoardContent);
        }
    }
}