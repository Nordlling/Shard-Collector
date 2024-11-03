using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.Toolkit.Scene
{
    public class SceneLoader : ISceneLoader
    {
        private float _sceneLoadProgress;
        private const float ProgressValueToAllowSceneActivation = 0.89f;

        public string CurrentSceneName => SceneManager.GetActiveScene().name;
        public AsyncOperation LoadSceneOperation { get; private set; }
        public event Action OnSceneLoaded;

        public async UniTask Load(string name, bool allowSceneActivation = true)
        {
            if (CurrentSceneName == name)
            {
               return;
            }
            
            LoadSceneOperation = SceneManager.LoadSceneAsync(name);
            if (LoadSceneOperation == null)
            {
                return;
            }
            LoadSceneOperation.allowSceneActivation = false;
            while (!LoadSceneOperation.isDone)
            {
                _sceneLoadProgress = LoadSceneOperation.progress;
            
                if (!LoadSceneOperation.allowSceneActivation && _sceneLoadProgress >= ProgressValueToAllowSceneActivation)
                {
                    if (!allowSceneActivation)
                    {
                        OnSceneLoaded?.Invoke();
                        return;
                    }
                    
                    LoadSceneOperation.allowSceneActivation = true;
                }
            
                await UniTask.Yield();
            }
        }
        
        public async UniTask LoadAdditive(string name)
        {;
            var loadSceneOperation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            if (loadSceneOperation == null)
            {
                return;
            }
            loadSceneOperation.allowSceneActivation = false;
            
            while (!loadSceneOperation.isDone)
            {
                _sceneLoadProgress = loadSceneOperation.progress;
            
                if (!loadSceneOperation.allowSceneActivation && _sceneLoadProgress >= ProgressValueToAllowSceneActivation)
                {
                    loadSceneOperation.allowSceneActivation = true;
                }
            
                await UniTask.Yield();
            }
        }

        public async UniTask UnloadScene(string name)
        {
            await SceneManager.UnloadSceneAsync(name);
        }
    }
}