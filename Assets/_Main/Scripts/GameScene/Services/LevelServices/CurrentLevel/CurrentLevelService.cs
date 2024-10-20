namespace _Main.Scripts.GameScene.Services
{
    public class CurrentLevelService : ICurrentLevelService
    {
        private readonly ILevelLoadService _levelLoadService;
        private readonly LevelInfo _currentLevel;

        public CurrentLevelService(ILevelLoadService levelLoadService)
        {
            _levelLoadService = levelLoadService;
            _currentLevel = _levelLoadService.GetDefaultLevel();
        }

        public LevelInfo GetCurrentLevel()
        {
            return _currentLevel;
        }
    }
}