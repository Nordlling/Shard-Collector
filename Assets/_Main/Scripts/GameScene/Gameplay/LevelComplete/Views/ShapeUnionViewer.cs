using System.Collections.Generic;
using Clipper2Lib;
using UnityEngine;

namespace _Main.Scripts
{
    // for test view
    public class ShapeUnionViewer : MonoBehaviour
    {
        [SerializeField] private bool show = true;

        public List<PathD> ShapeSolution = new();

        private void OnDrawGizmos()
        {
            if (!show)
            {
                return;
            }

            Gizmos.color = Color.red;
            foreach (var path in ShapeSolution)
            {
                int vertexCount = path.Count;
                for (int i = 0; i < vertexCount; i++)
                {
                    int nextIndex = (i + 1) % vertexCount;
                    Gizmos.DrawLine(new Vector3((float)path[i].x, (float)path[i].y, 0f), new Vector3((float)path[nextIndex].x, (float)path[nextIndex].y, 0f));
                }
            }
        }
        
    }
}