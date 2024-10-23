namespace _Main.Scripts.GameScene.Services
{
    public class CurrentLevelService : ICurrentLevelService
    {
        private readonly ILevelLoadService _levelLoadService;
        private LevelInfo _currentLevel;
        private LevelInfo _defaultLevel;
        private int _levelId;

        public CurrentLevelService(ILevelLoadService levelLoadService)
        {
            _levelLoadService = levelLoadService;
            _defaultLevel = _levelLoadService.GetDefaultLevel();
            _levelId = -1;
        }

        public LevelInfo GetCurrentLevel(bool canReturnDefault = true)
        {
            return canReturnDefault ? _currentLevel ?? _defaultLevel : _currentLevel;
        }

        public void LevelUp()
        {
            _levelId++;
            _currentLevel = _levelLoadService.GetLevelByLevelId(_levelId);
        }
        
    }
}