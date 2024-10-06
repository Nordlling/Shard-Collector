using UnityEngine;

namespace _Main.Scripts
{
    [CreateAssetMenu(menuName = "Configs/ShapeRender", fileName = "ShapeRenderConfig")]
    public class RenderConfig : ScriptableObject
    {
        public Material LineMaterial;
        public Material ShapeMaterial;
        public Material PatternMaterial;
    }
}