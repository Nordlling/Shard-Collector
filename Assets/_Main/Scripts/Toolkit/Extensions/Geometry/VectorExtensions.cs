using UnityEngine;

namespace _Main.Scripts 
{
    public static class VectorExtensions
    {
        public static bool IsInsideMesh(this Vector2 point, Mesh mesh, Transform meshTransform)
        {
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector2 v1 = meshTransform.TransformPoint(vertices[triangles[i]]);
                Vector2 v2 = meshTransform.TransformPoint(vertices[triangles[i + 1]]);
                Vector2 v3 = meshTransform.TransformPoint(vertices[triangles[i + 2]]);

                if (IsPointInsideTriangle(point, v1, v2, v3))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsPointInsideTriangle(Vector2 point, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            float d1 = Sign(point, v1, v2);
            float d2 = Sign(point, v2, v3);
            float d3 = Sign(point, v3, v1);

            bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(hasNeg && hasPos);
        }

        private static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }
    }
}