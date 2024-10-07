using _Main.Scripts.Gameplay.Painter;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.GameScene.MonoInstallers
{
    public class PainterInstaller : MonoInstaller
    {
        [SerializeField] private PainterView painterView;
        
        public override void InstallBindings()
        {
            Container.Bind<PainterView>().FromInstance(painterView).AsSingle();
        }

        
    }
}