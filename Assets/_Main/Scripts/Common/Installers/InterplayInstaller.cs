using _Main.Scripts.Common.InputSystem;
using _Main.Scripts.Toolkit.Screen;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Main.Scripts.Common.Installers
{
    public class InterplayInstaller : MonoInstaller
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private EventSystem eventSystem;
        
        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(mainCamera).AsCached();
            Container.Bind<EventSystem>().FromInstance(eventSystem).AsCached();
            Container.Bind<IScreenService>().To<ScreenService>().AsSingle();
            Container.Bind<IInputService>().To<InputService>().AsSingle();
        }
    }
}