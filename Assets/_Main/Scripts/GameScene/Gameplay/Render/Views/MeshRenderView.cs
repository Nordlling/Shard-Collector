using mattatz.Triangulation2DSystem;
using Scellecs.Morpeh;
using UnityEngine;
using Zenject;

namespace _Main.Scripts
{
    public class MeshRenderView : MonoBehaviour
    {
        private RenderConfig _renderConfig;
        private Entity _entity;

        [Inject]
        public void Construct(RenderConfig renderConfig)
        {
            _renderConfig = renderConfig;
        }

        public void Init(Entity entity)
        {
            _entity = entity;
        }
        
        private void OnRenderObject()
        {
            if (_entity == null)
            {
                return;
            }
            
            var shapeComponent = _entity.GetComponent<ShapeComponent>();
            
            if (shapeComponent.Triangles == null)
            {
                return;
            }

            GL.PushMatrix();
            GL.MultMatrix(shapeComponent.ShapeView.transform.localToWorldMatrix);

            _renderConfig.LineMaterial.SetColor("_Color", Color.black);
            _renderConfig.LineMaterial.SetPass(0);
			
            GL.Begin(GL.LINES);
            for (int i = 0; i < shapeComponent.Triangles.Length; i++)
            {
                Triangle2D triangle = shapeComponent.Triangles[i];
                GL.Vertex(triangle.s0.a.Coordinate); GL.Vertex(triangle.s0.b.Coordinate);
                GL.Vertex(triangle.s1.a.Coordinate); GL.Vertex(triangle.s1.b.Coordinate);
                GL.Vertex(triangle.s2.a.Coordinate); GL.Vertex(triangle.s2.b.Coordinate);
            }

            GL.End();
            GL.PopMatrix();
        }
        
    }
}