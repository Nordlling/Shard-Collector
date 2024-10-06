using mattatz.Triangulation2DSystem;
using UnityEngine;

namespace _Main.Scripts
{
    public static class TriangleExtensions
    {
        public static bool HasPosition(this Triangle2D triangle, Vector2 position)
        {
            return triangle.a.Coordinate == position || 
                   triangle.b.Coordinate == position ||
                   triangle.c.Coordinate == position;
        }
        
        public static int ContactPointsCount(this Triangle2D firstTriangle, Vector2 position)
        {
            int count = 0;
            count += firstTriangle.a.Coordinate == position ? 1 : 0;
            count += firstTriangle.b.Coordinate == position ? 1 : 0;
            count += firstTriangle.c.Coordinate == position ? 1 : 0;
            return count;
        }
        
        public static int ContactPointsCount(this Triangle2D firstTriangle, Triangle2D secondTriangle)
        {
            int count = 0;
            
            count += firstTriangle.HasPosition(secondTriangle.a.Coordinate) ? 1 : 0;
            count += firstTriangle.HasPosition(secondTriangle.b.Coordinate) ? 1 : 0;
            count += firstTriangle.HasPosition(secondTriangle.c.Coordinate) ? 1 : 0;
            
            return count;
        }
    }
}