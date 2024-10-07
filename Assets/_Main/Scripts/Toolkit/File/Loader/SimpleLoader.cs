using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Main.Scripts.Infrastructure.Services.GameGrid.Loader
{
    public class SimpleLoader : ISimpleLoader
    {
        public string LoadTextFile(string path)
        {
            try
            {
                return Resources.Load<TextAsset>(path).text;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error load file: {e.Message}");
            }

            return null;
        }
        
        public string[] LoadAllTextFiles(string path)
        {
            try
            {
                return Resources.LoadAll<TextAsset>(path).Select(e => e.text).ToArray();;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error load files: {e.Message}");
            }

            return null;
        }

        public Sprite LoadImage(string path)
        {
            try
            {
                return Resources.Load<Sprite>(path);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error load image: {e.Message}");
            }

            return null;
            
        }
        
        public T LoadFile<T>(string path) where T : Object
        {
            try
            {
                return Resources.Load<T>(path);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error load file: {e.Message}");
            }

            return null;
        }
        
    }
}