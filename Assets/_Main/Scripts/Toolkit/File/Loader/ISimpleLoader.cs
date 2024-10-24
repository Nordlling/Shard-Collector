using UnityEngine;

namespace _Main.Scripts.Toolkit.File
{
    public interface ISimpleLoader
    {
        T LoadFile<T>(string path) where T : Object;
        string LoadTextFile(string path);
        string[] LoadAllTextFiles(string path);
        Sprite LoadImage(string path);
    }
}