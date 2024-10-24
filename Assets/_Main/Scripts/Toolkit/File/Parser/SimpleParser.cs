using System;
using Newtonsoft.Json;
using UnityEngine;

namespace _Main.Scripts.Toolkit.File
{
    public class SimpleParser : ISimpleParser
    {
        public T ParseFromText<T>(string json) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error parsing JSON: {e.Message}");
                return null;
            }
        }

        public string ParseToText<T>(T value) where T : class
        {
            try
            {
                return JsonConvert.SerializeObject(value);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error parsing Object: {e.Message}");
            }

            return null;
        }
    }
}