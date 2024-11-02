using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace _Main.Scripts.Toolkit.EditorTools.Scenes
{
    public static class SceneNameHelper
    {
        public static List<string> GetSceneNames()
        {
#if UNITY_EDITOR
            return EditorBuildSettings.scenes.Select(scene => System.IO.Path.GetFileNameWithoutExtension(scene.path)).ToList();
#endif
            return new List<string>();
        }
    }
}