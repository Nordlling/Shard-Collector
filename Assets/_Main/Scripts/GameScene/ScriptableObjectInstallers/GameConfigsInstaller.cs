using _Main.Scripts.Pattern;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.GameScene.ScriptableObjectInstallers
{
    [CreateAssetMenu(menuName = "Configs/Installers/GameScenes/GameConfigsInstaller", fileName = "GameConfigsInstaller")]
    public class GameConfigsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private PatternDrawingConfig patternDrawingConfig;
        [SerializeField] private ShapeDragAndDropConfig shapeDragAndDropConfig;
        [SerializeField] private RenderConfig renderConfig;
        [SerializeField] private ShapeSelectorConfig shapeSelectorConfig;

        public override void InstallBindings()
        {
            Container.Bind<PatternDrawingConfig>().FromInstance(patternDrawingConfig).AsSingle();
            Container.Bind<ShapeDragAndDropConfig>().FromInstance(shapeDragAndDropConfig).AsSingle();
            Container.Bind<RenderConfig>().FromInstance(renderConfig).AsSingle();
            Container.Bind<ShapeSelectorConfig>().FromInstance(shapeSelectorConfig).AsSingle();
        }
    }
}