using System;
using Scellecs.Morpeh;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Components
{
    [Serializable]
    public struct AllShapesInSelectorSignal : IComponent
    {
        public float Delay;
        public bool InMove;
    }
}