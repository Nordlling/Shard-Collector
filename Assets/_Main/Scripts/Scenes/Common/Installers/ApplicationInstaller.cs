using _Main.Scripts.Global.Application.Configs;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Global.Installers
{
    public class ApplicationInstaller : MonoInstaller
    {
        [SerializeField] private ApplicationConfig applicationConfig;
		
        public override void InstallBindings()
        {
            SetApplicationSettings();
        }

        private void SetApplicationSettings()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            QualitySettings.vSyncCount = applicationConfig.vSyncCount;
            UnityEngine.Application.targetFrameRate = applicationConfig.systemTargetFrameRate;
            Input.multiTouchEnabled = applicationConfig.multiTouchEnabled;
			
            DOTween.SetTweensCapacity(applicationConfig.TweenersCapacity, applicationConfig.SequencesCapacity);
        }
    }
}