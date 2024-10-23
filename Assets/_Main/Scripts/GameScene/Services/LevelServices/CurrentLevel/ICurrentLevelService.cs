
namespace _Main.Scripts.GameScene.Services
{
    public interface ICurrentLevelService
    {
        LevelInfo GetCurrentLevel(bool canReturnDefault = true);
        void LevelUp();
    }
}