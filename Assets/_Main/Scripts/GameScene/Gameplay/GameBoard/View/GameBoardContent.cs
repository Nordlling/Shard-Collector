using UnityEngine;

namespace _Main.Scripts.GameScene.MonoInstallers
{
    public class GameBoardContent : MonoBehaviour
    {
        [SerializeField] private Transform patternContent;
        [SerializeField] private Transform shapesContent;
        [SerializeField] private Transform shapeSelectorContent;

        public Transform PatternContent => patternContent;
        public Transform ShapesContent => shapesContent;
        public Transform ShapeSelectorContent => shapeSelectorContent;
    }
}