using App.Scripts.Scenes.Game.Configs.Pool;
using Main.Scripts.Infrastructure.Services.GameGrid.Loader;
using Main.Scripts.Infrastructure.Services.GameGrid.Parser;

namespace _Main.Scripts.GameScene.Services
{
    public class LevelLoadService : ILevelLoadService
    {
        private readonly LevelConfig _levelConfig;
        private readonly ISimpleLoader _simpleLoader;
        private readonly ISimpleParser _simpleParser;

        public LevelLoadService(LevelConfig levelConfig, ISimpleLoader simpleLoader, ISimpleParser simpleParser)
        {
            _levelConfig = levelConfig;
            _simpleLoader = simpleLoader;
            _simpleParser = simpleParser;
        }

        public LevelInfo GetDefaultLevel()
        {
            string levelData = _simpleLoader.LoadTextFile(_levelConfig.DefaultLevelPath);
            LevelInfo levelInfo = _simpleParser?.ParseText<LevelInfo>(levelData);
            return levelInfo;
        }
    }
}