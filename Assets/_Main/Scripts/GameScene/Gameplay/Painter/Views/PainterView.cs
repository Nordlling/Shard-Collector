using System.Collections.Generic;
using App.Scripts.Modules.Pool.Abstract;
using UnityEngine;

namespace _Main.Scripts.Gameplay.Painter
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
            lineMaterial.SetColor("_Color", Color.white);
            lineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            for (int i = 0, n = _points.Count - 1; i < n; i++)
            {
                GL.Vertex(_points[i]); GL.Vertex(_points[i + 1]);
            }
            GL.End();
            GL.PopMatrix();
        }

    }
}