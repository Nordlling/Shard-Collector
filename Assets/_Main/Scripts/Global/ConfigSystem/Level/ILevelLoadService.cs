
using _Main.Scripts.Global.ConfigSystem.Level.Data;

namespace _Main.Scripts.Global.ConfigSystem.Level
{
    public interface ILevelLoadService
    {
        LevelInfo GetDefaultLevel();
        LevelInfo GetLevelByLevelId(int levelId);
    }
}