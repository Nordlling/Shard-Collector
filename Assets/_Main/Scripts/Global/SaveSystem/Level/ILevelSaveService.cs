using _Main.Scripts.Global.SaveSystem.Level.Data;

namespace _Main.Scripts.Global.SaveSystem.Level
{
    public interface ILevelSaveService
    {
        CurrentLevelSaveData LoadCurrentLevelData();
        void SaveCurrentLevelData(CurrentLevelSaveData levelSaveData);
    }
}