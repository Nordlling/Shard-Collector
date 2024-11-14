using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Render.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameScene/ShapeRender", fileName = "ShapeRenderConfig")]
    public class RenderConfig : ScriptableObject
    {
        public bool ShowLinesOnPattern;
        public Material LineMaterial;
        public Material ShapeMaterial;
        public Material PatternMaterial;
    }
}