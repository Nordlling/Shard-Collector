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

        public static float GetArea(this Triangle2D triangle)
        {
            float x1 = triangle.a.Coordinate.x, y1 = triangle.a.Coordinate.y;
            float x2 = triangle.b.Coordinate.x, y2 = triangle.b.Coordinate.y;
            float x3 = triangle.c.Coordinate.x, y3 = triangle.c.Coordinate.y;

            float area = Mathf.Abs(x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2)) / 2f;
            return area;
        }

        public static void SortVerticesClockwise(this Triangle2D triangle)
        {
            float crossProduct = (triangle.b.Coordinate.x - triangle.a.Coordinate.x) * (triangle.c.Coordinate.y - triangle.a.Coordinate.y) - 
                                 (triangle.b.Coordinate.y - triangle.a.Coordinate.y) * (triangle.c.Coordinate.x - triangle.a.Coordinate.x);
                
            var vertex0 = triangle.a;
            var vertex1 = crossProduct < 0 ? triangle.b : triangle.c;
            var vertex2 = crossProduct < 0 ? triangle.c : triangle.b;

            triangle.a = vertex0;
            triangle.b = vertex1;
            triangle.c = vertex2;
            
            triangle.s0.a = vertex0;
            triangle.s0.b = vertex1;
            triangle.s1.a = vertex1;
            triangle.s1.b = vertex2;
            triangle.s2.a = vertex2;
            triangle.s2.b = vertex0;
        }
        
        public static void SortVerticesClockwise(this Triangulation2D triangulation)
        {
            foreach (var triangle in triangulation.Triangles)
            {
                triangle.SortVerticesClockwise();
            }
        }
        
    }
}