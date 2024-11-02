using _Main.Scripts.Global.DialogSystem.Configs;
using _Main.Scripts.Global.DialogSystem.Services;
using _Main.Scripts.Global.Pool.Container;
using _Main.Scripts.Global.Pool.Extensions;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Global.Installers
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