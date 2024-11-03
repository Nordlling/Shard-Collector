using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Configs;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Painter.Views
{
    public class PainterButton : MonoBehaviour
    {
        private PatternDrawingConfig _patternDrawingConfig;

        [Inject]
        public void Construct(PatternDrawingConfig patternDrawingConfig)
        {
            _patternDrawingConfig = patternDrawingConfig;
        }
        
        [UsedImplicitly]
        public void OnClick()
        {
            _patternDrawingConfig.Enabled = !_patternDrawingConfig.Enabled;
        }
    }
}