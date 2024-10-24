namespace _Main.Scripts.GameScene.Services
{
    public interface ILevelSaveService
    {
        LevelSaveData LoadCurrentLevelData();
        void SaveCurrentLevelData(LevelSaveData levelSaveData);
    }
}