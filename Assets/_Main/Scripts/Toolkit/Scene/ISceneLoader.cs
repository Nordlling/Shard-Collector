using Cysharp.Threading.Tasks;

namespace _Main.Scripts.Toolkit.Scene
{
    public interface ISceneLoader
    {
        UniTask Load(string name);
    }
}