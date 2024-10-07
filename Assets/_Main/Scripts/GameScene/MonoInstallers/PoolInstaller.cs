using App.Scripts.Modules.Pool.Container;
using App.Scripts.Modules.Pool.Extensions;
using App.Scripts.Scenes.Game.Configs.Pool;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.GameScene.MonoInstallers
{
    public class PoolInstaller : MonoInstaller
    {
        [SerializeField] private GamePoolPrefabsConfig config;
        [SerializeField] private MonoPoolParentContainer poolParentContainer;
        
        public override void InstallBindings()
        {
            Container.BindMonoPool<ShapeView>(config.shapePoolData, poolParentContainer);
        }
    }
}