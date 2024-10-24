using _Main.Scripts.Toolkit.File;

namespace _Main.Scripts.GameScene.Services
{
    public class LevelSaveService : ILevelSaveService
    {
        private readonly IStorageService _storageService;
        private readonly ISimpleParser _parserService;
        private readonly ISaveKeysContainer _keysContainer;

        private LevelSaveData _cashedLevelSaveData;

        public LevelSaveService(IStorageService storageService, ISimpleParser parserService, ISaveKeysContainer keysContainer)
        {
            _storageService = storageService;
            _parserService = parserService;
            _keysContainer = keysContainer;
        }

        public LevelSaveData LoadCurrentLevelData()
        {
            if (_cashedLevelSaveData != null)
            {
                return _cashedLevelSaveData;
            }
            
            var plainData = _storageService.GetString(_keysContainer.LevelInfoKey);
            LevelSaveData resultData = _parserService.ParseFromText<LevelSaveData>(plainData);
            return resultData ?? new LevelSaveData();
        }

        public void SaveCurrentLevelData(LevelSaveData levelSaveData)
        {
            var plainData = _parserService.ParseToText(levelSaveData);
            _storageService.SetString(_keysContainer.LevelInfoKey, plainData);
            _cashedLevelSaveData = levelSaveData;
        }
    }
}