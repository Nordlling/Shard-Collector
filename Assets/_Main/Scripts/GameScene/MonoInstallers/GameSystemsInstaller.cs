using _Main.Scripts.Gameplay.GameBoard;
using _Main.Scripts.Spawn;
using _Main.Scripts.Toolkit;
using App.Scripts.Modules.EcsWorld.Infrastructure.Services;
using App.Scripts.Modules.EcsWorld.Infrastructure.Systems;
using Scellecs.Morpeh;
using Zenject;

namespace _Main.Scripts.GameScene.MonoInstallers
{
    public class GameSystemsInstaller : MonoInstaller
    {
        
        public override void InstallBindings()
        {
            Container.Bind<IWorldRunner>().To<WorldRunner>().AsSingle();
            Container.Bind<ISystemGroupContainer>().To<SystemGroupContainer>().AsSingle();

            Container.Bind<ISystem>().To<GameBoardSystem>().AsCached();
            Container.Bind<ISystem>().To<PainterSystem>().AsCached();
            Container.Bind<ISystem>().To<PatternSpawnSystem>().AsCached();
            Container.Bind<ISystem>().To<ShapeSpawnSystem>().AsCached();
            Container.Bind<ISystem>().To<ShapeSelectorFillSystem>().AsCached();
            Container.Bind<ISystem>().To<ShapeDragAndDropSystem>().AsCached();
            Container.Bind<ISystem>().To<ShapeSelectorResortSystem>().AsCached();
            Container.Bind<ISystem>().To<ShapeSelectorMoveSystem>().AsCached();
            Container.Bind<ISystem>().To<LevelCompleteSystem>().AsCached();
            Container.Bind<ISystem>().To<ShapeDestroySystem>().AsCached();

            Container.Bind<ISystem>().To<CleanupEntitySystem<DestroyEntitySignal>>().AsCached();
            Container.Bind<ISystem>().To<CleanupComponentSystem<ShapeSpawnSignal>>().AsCached();
            Container.Bind<ISystem>().To<CleanupComponentSystem<CreatePatternSignal>>().AsCached();
            
            Container.Bind<GameBoardInitializer>().AsSingle();
        }
        
    }
}