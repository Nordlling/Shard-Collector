using Scellecs.Morpeh;

namespace _Main.Scripts.Global.Ecs.Cleanup.Systems
{
    public class CleanupComponentSystem<TComponent> : ICleanupSystem where TComponent : struct, IComponent
    {
        private Filter _filter;
        public Scellecs.Morpeh.World World { get; set; }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<TComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                entity.RemoveComponent<TComponent>();
            }
        }

        public void Dispose()
        {
        }
        
    }
}