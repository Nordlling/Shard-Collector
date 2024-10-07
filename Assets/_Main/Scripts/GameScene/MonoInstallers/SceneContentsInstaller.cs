using UnityEngine;
using Zenject;

namespace _Main.Scripts.GameScene.MonoInstallers
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