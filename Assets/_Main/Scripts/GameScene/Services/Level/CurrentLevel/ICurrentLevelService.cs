
namespace _Main.Scripts.GameScene.Services
{
    public interface ICurrentLevelService
    {
        LevelInfo GetCurrentLevel();
        void LevelUp();
    }
}