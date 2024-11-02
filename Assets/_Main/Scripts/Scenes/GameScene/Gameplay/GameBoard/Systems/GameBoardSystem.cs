using Scellecs.Morpeh;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.GameBoard.Systems
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