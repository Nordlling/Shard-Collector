using System;
using mattatz.Triangulation2DSystem;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Spawn
{
    [Serializable]
    public struct ShapeSpawnSignal : IComponent
    {
        public Transform Parent;
        public Vector3 Position;
        public Vector3 Size;
        public Triangle2D[] Triangles;
        public Vector3 PatternSize;
    }
}