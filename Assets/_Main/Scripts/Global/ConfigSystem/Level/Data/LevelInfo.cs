using System;
using System.Collections.Generic;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Data;
using UnityEngine;

namespace _Main.Scripts.Global.ConfigSystem.Level.Data
{
    [Serializable]
    public class LevelInfo
    {
        public ShapesGenerateInfo ShapesGenerateInfo;
        public int ExtraMoves;
        public List<Vector2> Points;
    }
}