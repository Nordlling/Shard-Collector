using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Gameplay.GameBoard
{
    [Serializable]
    public struct CreatePatternSignal : IComponent
    {
        public List<Vector2> Points;
    }
}