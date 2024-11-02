using System;
using _Main.Scripts.Scenes.GameScene.Gameplay.Painter.Views;
using Scellecs.Morpeh;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Painter.Components
{
    [Serializable]
    public struct PainterComponent : IComponent
    {
        public PainterView PainterView;
    }
}