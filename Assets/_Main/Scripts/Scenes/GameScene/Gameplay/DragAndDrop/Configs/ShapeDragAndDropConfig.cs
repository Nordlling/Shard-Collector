using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.DragAndDrop.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameScene/ShapeDragAndDrop", fileName = "ShapeDragAndDropConfig")]
    public class ShapeDragAndDropConfig : ScriptableObject
    {
        [Min(0)] public int MinOverlapCount;
        [Min(0)] public float MaxOverlapDistance = 1.5f;
        
        [Header("Animation")] 
        [Min(0)] public float ShadowMoveSpeed = 15f;
        [Min(0)] public float ShapeMoveToPatternDuration = 0.2f;
        [Min(0)] public float ShapeMoveToSelectorDuration = 0.3f;
        [Min(0)] public float ShapeScaleDuration = 0.3f;
    }
}