using _Main.Scripts.Toolkit.File;

namespace _Main.Scripts.GameScene.Services
{
    public class LevelSaveService : ILevelSaveService
    {
        private readonly IStorageService _storageService;
        private readonly ISimpleParser _parserService;
        private readonly ISaveKeysContainer _keysContainer;

        private CurrentLevelSaveData _cashedLevelSaveData;

        public LevelSaveService(IStorageService storageService, ISimpleParser parserService, ISaveKeysContainer keysContainer)
        {
            _storageService = storageService;
            _parserService = parserService;
            _keysContainer = keysContainer;
        }

        public CurrentLevelSaveData LoadCurrentLevelData()
        {
            if (_cashedLevelSaveData != null)
            {
                return _cashedLevelSaveData;
            }
            
            var plainData = _storageService.GetString(_keysContainer.LevelInfoKey);
            CurrentLevelSaveData resultData = _parserService.ParseFromText<CurrentLevelSaveData>(plainData);
            return resultData ?? new CurrentLevelSaveData();
        }

        public void SaveCurrentLevelData(CurrentLevelSaveData levelSaveData)
        {
            var plainData = _parserService.ParseToText(levelSaveData);
            _storageService.SetString(_keysContainer.LevelInfoKey, plainData);
            _cashedLevelSaveData = levelSaveData;
        }
    }
}