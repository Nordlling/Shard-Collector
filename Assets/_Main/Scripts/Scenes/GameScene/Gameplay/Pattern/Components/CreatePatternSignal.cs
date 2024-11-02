using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Components
{
    [Serializable]
    public struct CreatePatternSignal : IComponent
    {
        public List<Vector2> Points;
    }
}