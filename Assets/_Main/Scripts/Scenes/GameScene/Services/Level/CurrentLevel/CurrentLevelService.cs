using _Main.Scripts.Global.ConfigSystem.Level;
using _Main.Scripts.Global.ConfigSystem.Level.Data;
using _Main.Scripts.Global.SaveSystem.Level;
using _Main.Scripts.Global.SaveSystem.Level.Data;
using _Main.Scripts.Scenes.GameScene.Gameplay.LevelComplete.Configs;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Services.Level.CurrentLevel
{
    public class CurrentLevelService : ICurrentLevelService
    {
        private readonly ILevelLoadService _levelLoadService;
        private readonly ILevelSaveService _levelSaveService;
        private readonly LevelConfig _levelConfig;
        private readonly LevelCompleteConfig _levelCompleteConfig;
        private int _levelId;

        public bool CanLevelUp => LastCompletedLevel.Stars > 0;
        public CompletedLevelSaveData LastCompletedLevel { get; private set; } = new();
        public LevelInfo CurrentLevel { get; private set; }

        public CurrentLevelService(ILevelLoadService levelLoadService, ILevelSaveService levelSaveService, LevelConfig levelConfig, LevelCompleteConfig levelCompleteConfig)
        {
            _levelLoadService = levelLoadService;
            _levelSaveService = levelSaveService;
            _levelConfig = levelConfig;
            _levelCompleteConfig = levelCompleteConfig;

            LoadActualLevel();
        }

        public void FinishLevel(int percent)
        {
            LastCompletedLevel.LevelIndex = _levelId;
            LastCompletedLevel.Percent = percent;
            LastCompletedLevel.Stars = percent / _levelCompleteConfig.FirstStarFullPercent +
                                       percent / _levelCompleteConfig.SecondStarFullPercent +
                                       percent / _levelCompleteConfig.ThirdStarFullPercent;
            
        }
        
        public bool TryLevelUp()
        {
            if (!CanLevelUp)
            {
                return false;
            }
            
            while (true)
            {
                var newLevelId = _levelId + 1;
                _levelId = _levelConfig.LevelsMap.ContainsKey(newLevelId) ? newLevelId : _levelConfig.FirstLevelIndex;
                CurrentLevel = _levelLoadService.GetLevelByLevelId(_levelId);

                if (CurrentLevel == null)
                {
                    Debug.LogWarning($"Next levelId({_levelId}) is invalid. Try get another level");
                    continue;
                }

                var levelSaveData = _levelSaveService.LoadCurrentLevelData();
                levelSaveData.LevelIndex = _levelId;
                _levelSaveService.SaveCurrentLevelData(levelSaveData);
                return true;
            }
        }

        private void LoadActualLevel()
        {
            if (_levelConfig.UseDefaultLevel)
            {
                CurrentLevel = _levelLoadService.GetDefaultLevel();
                _levelId = -1;
                return;
            }
            
            var saveData = _levelSaveService.LoadCurrentLevelData();
            _levelId = saveData?.LevelIndex ?? _levelConfig.FirstLevelIndex;
            CurrentLevel = _levelLoadService.GetLevelByLevelId(_levelId);
        }
        
    }
}