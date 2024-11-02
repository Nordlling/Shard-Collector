using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using Scellecs.Morpeh;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Systems
{
    public class ShapeDestroySystem : ISystem
    {
        private Filter _filter;
        
        public World World { get; set; }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<ShapeDestroySignal>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var shapeComponent = ref entity.GetComponent<ShapeComponent>();
                shapeComponent.ShapeView.Remove();
                World.RemoveEntity(entity);
            }
        }

        public void Dispose() { }
    }
}