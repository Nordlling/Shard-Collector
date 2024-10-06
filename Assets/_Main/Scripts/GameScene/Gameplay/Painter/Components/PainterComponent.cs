using System;
using Scellecs.Morpeh;

namespace _Main.Scripts.Gameplay.Painter
{
    [Serializable]
    public struct PainterComponent : IComponent
    {
        public PainterView PainterView;
    }
}