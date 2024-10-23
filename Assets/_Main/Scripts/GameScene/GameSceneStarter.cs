using Zenject;

namespace _Main.Scripts.GameScene
{
    public class GameSceneStarter : IInitializable
    {
        private readonly IGameSceneCreator _gameSceneCreator;

        public GameSceneStarter(IGameSceneCreator gameSceneCreator)
        {
            _gameSceneCreator = gameSceneCreator;
        }

        public void Initialize()
        {
            _gameSceneCreator.Create();
        }
    }
}