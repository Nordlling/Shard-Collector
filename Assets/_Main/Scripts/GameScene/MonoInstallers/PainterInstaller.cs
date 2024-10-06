using _Main.Scripts.Gameplay.Painter;
using UnityEngine;
using Zenject;
namespace App.Scripts.Scenes.Game.LifeCycle.Installers
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