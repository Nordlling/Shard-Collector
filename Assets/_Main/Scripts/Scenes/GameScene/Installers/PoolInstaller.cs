using _Main.Scripts.Global.Pool.Container;
using _Main.Scripts.Global.Pool.Extensions;
using _Main.Scripts.Scenes.GameScene.Configs;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Views;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Installers
{
    public class PoolInstaller : MonoInstaller
    {
        [SerializeField] private GamePoolPrefabsConfig config;
        [SerializeField] private MonoPoolParentContainer poolContainer;
        
        public override void InstallBindings()
        {
            Container.BindMonoPool<ShapeView>(config.shapePoolData, poolContainer);
        }
    }
}