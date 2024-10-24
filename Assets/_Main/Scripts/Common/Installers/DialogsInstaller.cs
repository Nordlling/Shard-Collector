using App.Scripts.Modules.Dialogs.Interfaces;
using App.Scripts.Modules.Dialogs.Services;
using App.Scripts.Modules.Pool.Container;
using App.Scripts.Modules.Pool.Extensions;
using App.Scripts.Scenes.Game.Configs.Pool;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Common.Installers
{
    public class DialogsServicesInstaller : MonoInstaller
    {
        [SerializeField] private DialogsPoolPrefabsConfig config;
        [SerializeField] private MonoPoolParentContainer poolContainer;
        [SerializeField] private Transform activeDialogsContainer;

        public override void InstallBindings()
        {
            Container.BindMonoTypesPool<PoolableDialog>(config.dialogsPoolData, poolContainer);
            Container.Bind<IDialogsService>().To<DialogsService>().AsSingle().WithArguments(activeDialogsContainer);
        }
    }
}