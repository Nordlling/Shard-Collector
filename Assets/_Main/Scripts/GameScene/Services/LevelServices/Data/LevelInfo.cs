using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.GameScene.Services
{
    [Serializable]
    public class LevelInfo
    {
        public int ExtraMoves;
        public List<Vector2> Points;
    }
}