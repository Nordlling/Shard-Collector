using System;
using App.Scripts.Modules.Bootstrap.Application.Configs;
using App.Scripts.Modules.Utils.RandomService;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Common.Installers
{
    public class CommonInstaller : MonoInstaller
    {
		
        public override void InstallBindings()
        {
            Container.Bind<IRandomService>().To<SystemRandomService>().AsSingle().WithArguments(DateTime.Now.Millisecond);
        }
    }
}