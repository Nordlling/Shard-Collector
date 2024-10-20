using Scellecs.Morpeh;

namespace _Main.Scripts.Toolkit
{
    public class CleanupEntitySystem<TComponent> : ICleanupSystem where TComponent : struct, IComponent
    {
        private Filter _filter;
        public World World { get; set; }

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
                World.RemoveEntity(entity);
            }
        }

        public void Dispose()
        {
        }
        
    }
}