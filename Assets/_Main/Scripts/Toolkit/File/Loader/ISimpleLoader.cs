using UnityEngine;

namespace Main.Scripts.Infrastructure.Services.GameGrid.Loader
{
    public interface ISimpleLoader
    {
        T LoadFile<T>(string path) where T : Object;
        string LoadTextFile(string path);
        string[] LoadAllTextFiles(string path);
        Sprite LoadImage(string path);
    }
}