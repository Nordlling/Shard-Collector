using System;
using UnityEngine;

namespace _Main.Scripts.Toolkit.Polygon
{
    public struct Edge : IEquatable<Edge>
    {
        public Vector3 PointA;
        public Vector3 PointB;

        public Edge(Vector3 pointA, Vector3 pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }

        public bool Equals(Edge other)
        {
            return PointA == other.PointA && PointB == other.PointB || PointA == other.PointB && PointB == other.PointA;
        }
    }
}