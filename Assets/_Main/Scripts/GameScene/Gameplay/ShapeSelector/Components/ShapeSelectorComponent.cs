using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts 
{ 
    [Serializable]
    public struct ShapeSelectorComponent : IComponent
    {
        public Transform Transform;
        public Rect Rect;
        public float PaddingTop;
        public float PaddingBottom;
        public float CellOffset;
        public float CellSize;
        public int MaxCount;
    }
}