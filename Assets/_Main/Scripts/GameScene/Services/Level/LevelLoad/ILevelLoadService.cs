
namespace _Main.Scripts.GameScene.Services
{
    public interface ILevelLoadService
    {
        LevelInfo GetDefaultLevel();
        LevelInfo GetLevelByLevelId(int levelId);
    }
}