using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Main.Scripts.Infrastructure.Services.GameGrid.Parser
{
    public class SimpleParser : ISimpleParser
    {
        public T ParseText<T>(string json) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error parsing JSON: {e.Message}");
            }

            return null;
        }
        
    }
}