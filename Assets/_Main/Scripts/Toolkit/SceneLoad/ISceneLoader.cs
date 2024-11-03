using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Main.Scripts.Toolkit.Scene
{
    public interface ISceneLoader
    {
        event Action OnSceneLoaded;
        string CurrentSceneName { get; }
        AsyncOperation LoadSceneOperation { get; }
        UniTask Load(string name, bool allowSceneActivation = true);
        UniTask LoadAdditive(string name);
        UniTask UnloadScene(string name);
    }
}