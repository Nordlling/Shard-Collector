using _Main.Scripts;
using App.Scripts.Modules.Pool.Container;
using App.Scripts.Modules.Pool.Extensions;
using App.Scripts.Scenes.Game.Configs.Pool;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.Game.LifeCycle.Installers
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