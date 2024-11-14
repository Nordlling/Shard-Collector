using _Main.Scripts.Global.Ecs.Cleanup.Components;
using _Main.Scripts.Global.Ecs.Cleanup.Systems;
using _Main.Scripts.Global.Ecs.World;
using _Main.Scripts.Scenes.GameScene.Gameplay.DragAndDrop.Systems;
using _Main.Scripts.Scenes.GameScene.Gameplay.GameBoard.Systems;
using _Main.Scripts.Scenes.GameScene.Gameplay.LevelComplete.Systems;
using _Main.Scripts.Scenes.GameScene.Gameplay.Painter.Systems;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Systems;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Systems;
using _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Systems;
using _Main.Scripts.Scenes.GameScene.Services.Layer;
using _Main.Scripts.Toolkit.Polygon;
using Scellecs.Morpeh;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Installers
{
    public class GameSystemsInstaller : MonoInstaller
    {
        
        public override void InstallBindings()
        {
            Container.Bind<PolygonAreaCalculator>().AsSingle();
            Container.Bind<ShapeGrouper>().AsSingle();
            Container.Bind<ILayerService>().To<LayerService>().AsSingle();
            
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
            Container.Bind<ISystem>().To<CleanupComponentSystem<ShapeSelectorResortSignal>>().AsCached();
            Container.Bind<ISystem>().To<CleanupComponentSystem<CreatePatternSignal>>().AsCached();
            
            Container.Bind<GameBoardInitializer>().AsSingle();
        }
        
    }
}