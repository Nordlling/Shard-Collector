using UnityEngine;

namespace _Main.Scripts
{
    [CreateAssetMenu(menuName = "Configs/ShapeDragAndDrop", fileName = "ShapeDragAndDropConfig")]
    public class ShapeDragAndDropConfig : ScriptableObject
    {
        [Min(0)] public int MinOverlapCount;
        [Range(0.2f, 5f)] public float MaxOverlapDistance = 1.5f;
    }
}