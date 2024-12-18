using _Main.Scripts.Global.ConfigSystem.Level.Data;
using _Main.Scripts.Toolkit.File.Loader;
using _Main.Scripts.Toolkit.File.Parser;

namespace _Main.Scripts.Global.ConfigSystem.Level
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
            return GetLevelByPath(_levelConfig.DefaultLevelPath);
        }

        public LevelInfo GetLevelByLevelId(int levelId)
        {
            if (!_levelConfig.LevelsMap.TryGetValue(levelId, out var levelPath))
            {
                return null;
            }

            return GetLevelByPath(levelPath);
        }
        
        private LevelInfo GetLevelByPath(string path)
        {
            string levelData = _simpleLoader.LoadTextFile(path);
            LevelInfo levelInfo = _simpleParser.ParseFromText<LevelInfo>(levelData);
            return levelInfo;
        }
        
    }
}