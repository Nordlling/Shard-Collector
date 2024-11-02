using Zenject;

namespace _Main.Scripts.GameScene.MonoInstallers
{
    public class EcsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameSceneCreator>().AsSingle();
        }
    }
    
}