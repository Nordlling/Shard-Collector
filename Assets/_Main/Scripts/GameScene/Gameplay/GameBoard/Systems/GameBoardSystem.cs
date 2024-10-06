using App.Scripts.Modules.EcsWorld.Common.Extensions;
using Scellecs.Morpeh;

namespace _Main.Scripts.Gameplay.GameBoard
{
    public class GameBoardSystem : ISystem
    {
        private Filter _createPatternFilter;
        
        public World World { get; set; }

        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
        }
        
        public void Dispose() { }
        
    }
}