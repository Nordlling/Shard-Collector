using Zenject;

namespace _Main.Scripts.GameScene.MonoInstallers
{
    public class GameSceneStarterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameSceneCreator>().AsSingle();
            Container.BindInterfacesTo<GameSceneStarter>().AsSingle();
        }
    }
    
}