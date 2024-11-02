namespace _Main.Scripts.GameScene.Services
{
    public interface ILevelSaveService
    {
        CurrentLevelSaveData LoadCurrentLevelData();
        void SaveCurrentLevelData(CurrentLevelSaveData levelSaveData);
    }
}