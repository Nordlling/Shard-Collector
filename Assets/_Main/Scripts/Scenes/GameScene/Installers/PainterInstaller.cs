using _Main.Scripts.Scenes.GameScene.Gameplay.Painter.Views;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Installers
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