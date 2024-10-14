using _Main.Scripts.Toolkit.Screen;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Common.Installers
{
    public class InterplayInstaller : MonoInstaller
    {
        [SerializeField] private Camera mainCamera;
        
        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(mainCamera).AsCached();
            Container.Bind<IScreenService>().To<ScreenService>().AsSingle();
        }
    }
}