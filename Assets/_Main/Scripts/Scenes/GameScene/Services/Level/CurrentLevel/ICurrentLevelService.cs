
using _Main.Scripts.Global.ConfigSystem.Level.Data;
using _Main.Scripts.Global.SaveSystem.Level.Data;

namespace _Main.Scripts.Scenes.GameScene.Services.Level.CurrentLevel
{
    public interface ICurrentLevelService
    {
        bool CanLevelUp { get; }
        LevelInfo CurrentLevel { get; }
        CompletedLevelSaveData LastCompletedLevel { get; }
        void FinishLevel(int percent);
        bool TryLevelUp();
    }
}