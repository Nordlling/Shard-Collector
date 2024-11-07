using System;
using System.Collections.Generic;
using _Main.Scripts.Global.Pool.Abstract;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Painter.Views
{
    public class PainterView : MonoPoolableItem
    {
        [SerializeField] private Material lineMaterial;
        
        private List<Vector2> _points = new();

        public void SetPoints(List<Vector2> points)
        {
            _points = points;
        }

        private void OnRenderObject()
        {
            if (_points == null)
            {
                return;
            }

            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);
            lineMaterial.SetColor("_Color", Color.black);
            lineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            for (int i = 0, n = _points.Count - 1; i < n; i++)
            {
                GL.Vertex(_points[i]); GL.Vertex(_points[i + 1]);
            }
            GL.End();
            GL.PopMatrix();
        }

        [SerializeField] private bool show = true;
        public static List<Vector3[]> Layers = new();

        private void OnDrawGizmos()
        {
            if (!show)
            {
                return;
            }

            List<Color> colors = new List<Color>()
            {
                Color.red,
                Color.green,
                Color.blue,
                Color.yellow
            };

            for (int i = 0; i < Layers.Count; i++)
            {
                Gizmos.color = colors[i];
                var path = Layers[i];
                int vertexCount = path.Length;
                for (int j = 0; j < vertexCount; j++)
                {
                    int nextIndex = (j + 1) % vertexCount;
                    Gizmos.DrawLine(new Vector3((float)path[j].x, (float)path[j].y, 0f), new Vector3((float)path[nextIndex].x, (float)path[nextIndex].y, 0f));
                }
            }
        }
    }
}