using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.Toolkit.Scene
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask Load(string name)
        {
            if (SceneManager.GetActiveScene().name != name)
            {
                await SceneManager.LoadSceneAsync(name);
            }
        }
    }
}