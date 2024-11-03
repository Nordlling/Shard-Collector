using _Main.Scripts.Global.ConfigSystem.Level.Data;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Global.Installers
{
    [CreateAssetMenu(menuName = "Configs/Global/Installers/GlobalConfigsSOInstaller", fileName = "GlobalConfigsSOInstaller")]
    public class GlobalConfigsSOInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private LevelConfig levelConfig;

        public override void InstallBindings()
        {
            Container.Bind<LevelConfig>().FromInstance(levelConfig).AsSingle();
        }
    }
}