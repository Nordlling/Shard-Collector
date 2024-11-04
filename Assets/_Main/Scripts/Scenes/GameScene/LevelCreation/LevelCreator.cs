using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using _Main.Scripts.Global.ConfigSystem.Level.Data;
using _Main.Scripts.Scenes.GameScene.Gameplay.Painter.Views;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Configs;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.LevelCreation
{
    public class LevelCreator : SerializedMonoBehaviour
    {
        [ReadOnly] [TextArea] public string logMessage; 
        
        [SerializeField] private PainterView painterView;
        [SerializeField] private LevelConfig levelConfig;
        [SerializeField] private PatternDrawingConfig patternDrawingConfig;
        [SerializeField] private int levelId;
        [SerializeField] private bool useCustomLevelName;
        [SerializeField] private bool rewrite;
        [SerializeField, OnValueChanged(nameof(ChangeThreshold))] private float threshold;
        [SerializeField, ShowIf(nameof(useCustomLevelName))] private string customLevelName;
        [SerializeField] private SimpleLevelInfo levelInfo = new();

        private void Awake()
        {
            if (threshold > 0)
            {
                ChangeThreshold();
            }
            levelId = levelConfig.LevelsMap.Keys.Max(el => el) + 1;
        }

        private void ChangeThreshold()
        {
            patternDrawingConfig.Threshold = threshold;
        }

        private const string LevelNameTemplate = "Levels/Level{0}";
        private const string ResourcesCommonPath = "_Main/Resources";
        
        [Button]
        private void FillPoints()
        {
            logMessage = "";
            FieldInfo field = typeof(PainterView).GetField("_points", BindingFlags.NonPublic | BindingFlags.Instance);
            var points = field?.GetValue(painterView) as List<Vector2>;
            levelInfo.Points = points?.Select(el => new SimpleVector2(el.x, el.y)).ToList();
            if (points.IsNullOrEmpty())
            {
                logMessage += $"FAIL: Can't get points. field is null - {field == null} points is null - {points == null} points is Empty - {points?.Count == 0}\n";
                Debug.LogError(logMessage);
                return;
            }
            logMessage += $"SUCCESS: Found {points?.Count} points\n";
            Debug.Log(logMessage);
        }
        
        [Button]
        private void WriteLevelJson()
        {
            logMessage = "";
            if (levelInfo.Points.IsNullOrEmpty())
            {
                logMessage += $"FAIL: Points is empty\n";
                Debug.LogError(logMessage);
                return;
            }
            
            string levelName = useCustomLevelName ? customLevelName : string.Format(LevelNameTemplate, levelId);
            string json = JsonConvert.SerializeObject(levelInfo, Formatting.Indented);
            string folderPath = Path.Combine(Application.dataPath, ResourcesCommonPath);
            if (!Directory.Exists(folderPath))
            {
                logMessage += $"WARN: FolderPath: {folderPath} don't exist. Create folder\n";
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, levelName) + ".json";
            
            if (File.Exists(filePath))
            {
                if (rewrite)
                {
                    logMessage += $"WARN: filePath: {filePath} already exists. Rewrite is enabled\n";
                }
                else
                {
                    logMessage += $"FAIL: filePath: {filePath} already exists. Rewrite is disabled\n";
                    Debug.LogError(logMessage);
                    return;
                }
            }
            
            File.WriteAllText(filePath, json);
            logMessage += $"SUCCESS: JSON file saved at: {filePath}\n";
            Debug.Log(logMessage);
        }
        
#if UNITY_EDITOR
        [Button]
        private void UpdateConfig()
        {
            logMessage = "";
            if (levelConfig.LevelsMap.ContainsKey(levelId))
            {
                if (rewrite)
                {
                    logMessage += $"WARN: Level {levelId} already exists. Rewrite is enabled\n";
                }
                else
                {
                    logMessage += $"FAIL Level {levelId} already exists. Rewrite is disabled\n";
                    Debug.LogError(logMessage);
                    return;
                }
            }
            
            string levelName = useCustomLevelName ? customLevelName : string.Format(LevelNameTemplate, levelId);
            levelConfig.LevelsMap[levelId] = levelName;
            EditorUtility.SetDirty(levelConfig);
            AssetDatabase.SaveAssets();
            logMessage += $"SUCCESS: Config updated to {AssetDatabase.GetAssetPath(levelConfig)}\n";
            Debug.Log(logMessage);
        }
#endif        

    }

    [Serializable]
    internal struct SimpleVector2
    {
        public float x;
        public float y;

        public SimpleVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
    
    [Serializable]
    internal class SimpleLevelInfo
    {
        public int ExtraMoves;
        public List<SimpleVector2> Points;
    }
}