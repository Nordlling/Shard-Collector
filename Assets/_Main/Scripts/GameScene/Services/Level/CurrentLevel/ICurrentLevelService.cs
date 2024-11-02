
namespace _Main.Scripts.GameScene.Services
{
    public interface ICurrentLevelService
    {
        bool CanLevelUp { get; }
        CompletedLevelSaveData LastCompletedLevel { get; }
        LevelInfo GetCurrentLevel();
        void FinishLevel(int percent);
        bool TryLevelUp();
    }
}