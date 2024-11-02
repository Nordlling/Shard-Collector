using _Main.Scripts.Scenes.GameScene.Gameplay.DragAndDrop.Configs;
using _Main.Scripts.Scenes.GameScene.Gameplay.LevelComplete.Configs;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Configs;
using _Main.Scripts.Scenes.GameScene.Gameplay.Render.Configs;
using _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Configs;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Installers
{
    [CreateAssetMenu(menuName = "Configs/GameScene/Installers/GameSceneConfigsSOInstaller", fileName = "GameSceneConfigsSOInstaller")]
    public class GameSceneConfigsSOInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private PatternDrawingConfig patternDrawingConfig;
        [SerializeField] private ShapeDragAndDropConfig shapeDragAndDropConfig;
        [SerializeField] private RenderConfig renderConfig;
        [SerializeField] private ShapeSelectorConfig shapeSelectorConfig;
        [SerializeField] private GenerateConfig generateConfig;
        [SerializeField] private LevelCompleteConfig levelCompleteConfig;

        public override void InstallBindings()
        {
            Container.Bind<PatternDrawingConfig>().FromInstance(patternDrawingConfig).AsSingle();
            Container.Bind<ShapeDragAndDropConfig>().FromInstance(shapeDragAndDropConfig).AsSingle();
            Container.Bind<RenderConfig>().FromInstance(renderConfig).AsSingle();
            Container.Bind<ShapeSelectorConfig>().FromInstance(shapeSelectorConfig).AsSingle();
            Container.Bind<GenerateConfig>().FromInstance(generateConfig).AsSingle();
            Container.Bind<LevelCompleteConfig>().FromInstance(levelCompleteConfig).AsSingle();
        }
    }
}