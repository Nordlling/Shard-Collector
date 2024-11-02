using UnityEngine;

namespace _Main.Scripts.Toolkit.Extensions.Geometry
{
    public static class MeshExtensions
    {
        public static void FillUV(this Mesh mesh)
        {
            Vector3[] vertices = mesh.vertices;
            Vector2[] uv = new Vector2[vertices.Length];

            Bounds bounds = mesh.bounds;
            Vector3 min = bounds.min;
            Vector3 size = bounds.size;

            for (int i = 0; i < vertices.Length; i++)
            {
                uv[i] = new Vector2((vertices[i].x - min.x) / size.x, (vertices[i].y - min.y) / size.y);
            }
            
            mesh.uv = uv;
        }
        
    }
}