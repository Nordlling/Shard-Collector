using _Main.Scripts;
using _Main.Scripts.Gameplay.GameBoard;
using _Main.Scripts.Spawn;
using App.Scripts.Modules.EcsWorld.Infrastructure.Services;
using App.Scripts.Modules.EcsWorld.Infrastructure.Systems;
using App.Scripts.Modules.Pool.Container;
using App.Scripts.Modules.Pool.Extensions;
using App.Scripts.Scenes.Game.Configs.Pool;
using Scellecs.Morpeh;
using UnityEngine;
using Zenject;
namespace App.Scripts.Scenes.Game.LifeCycle.Installers
{
    public class GameSystemsInstaller : MonoInstaller
    {
        
        public override void InstallBindings()
        {
            Container.Bind<IWorldRunner>().To<WorldRunner>().AsSingle();
            Container.Bind<ISystemGroupContainer>().To<SystemGroupContainer>().AsSingle();
            
            Container.Bind<ISystem>().To<GameBoardSystem>().AsCached();
            Container.Bind<ISystem>().To<PatternSpawnSystem>().AsCached();
            Container.Bind<ISystem>().To<ShapeSpawnSystem>().AsCached();
            Container.Bind<ISystem>().To<ShapeDragAndDropSystem>().AsCached();
            Container.Bind<ISystem>().To<PainterSystem>().AsCached();
            Container.Bind<ISystem>().To<ShapeDestroySystem>().AsCached();
        }
        
    }
}