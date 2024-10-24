using App.Scripts.Scenes.Game.Configs.Pool;
using UnityEngine;

namespace _Main.Scripts.GameScene.Services
{
    public class CurrentLevelService : ICurrentLevelService
    {
        private readonly ILevelLoadService _levelLoadService;
        private readonly ILevelSaveService _levelSaveService;
        private readonly LevelConfig _levelConfig;
        private LevelInfo _currentLevel;
        private int _levelId;
        
        public CurrentLevelService(ILevelLoadService levelLoadService, ILevelSaveService levelSaveService, LevelConfig levelConfig)
        {
            _levelLoadService = levelLoadService;
            _levelSaveService = levelSaveService;
            _levelConfig = levelConfig;

            LoadActualLevel();
        }

        public LevelInfo GetCurrentLevel()
        {
            return _currentLevel;
        }

        public void LevelUp()
        {
            var newLevelId = _levelId + 1;
            _levelId = _levelConfig.LevelsMap.ContainsKey(newLevelId) ? newLevelId : _levelConfig.FirstLevelIndex;
            _currentLevel = _levelLoadService.GetLevelByLevelId(_levelId);
            
            if (_currentLevel == null)
            {
                Debug.LogWarning($"Next levelId({_levelId}) is invalid. Try get another level");
                LevelUp();
                return;
            }
            
            var levelSaveData = _levelSaveService.LoadCurrentLevelData();
            levelSaveData.LevelIndex = _levelId;
            _levelSaveService.SaveCurrentLevelData(levelSaveData);
        }

        private void LoadActualLevel()
        {
            if (_levelConfig.UseDefaultLevel)
            {
                _currentLevel = _levelLoadService.GetDefaultLevel();
                _levelId = -1;
                return;
            }
            
            var saveData = _levelSaveService.LoadCurrentLevelData();
            _levelId = saveData?.LevelIndex ?? _levelConfig.FirstLevelIndex;
            _currentLevel = _levelLoadService.GetLevelByLevelId(_levelId);
        }
        
    }
}