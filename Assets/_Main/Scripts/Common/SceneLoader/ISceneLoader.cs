using Cysharp.Threading.Tasks;

namespace _Main.Scripts.Common
{
    public interface ISceneLoader
    {
        UniTask Load(string name);
    }
}