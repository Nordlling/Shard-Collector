using _Main.Scripts.Spawn;
using Zenject;

namespace App.Scripts.Scenes.Game.LifeCycle.Installers
{
    public class GameSceneStarterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameSceneStarter>().AsSingle();
        }
    }
    
}