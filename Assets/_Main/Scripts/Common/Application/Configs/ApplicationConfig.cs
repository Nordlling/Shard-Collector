using Sirenix.OdinInspector;
using UnityEngine;
namespace App.Scripts.Modules.Bootstrap.Application.Configs
{
	[CreateAssetMenu(menuName = "Configs/Services/Project/ApplicationSettingsConfig", fileName = "ApplicationSettingsConfig")]
	public class ApplicationConfig : ScriptableObject
	{
		public int vSyncCount = 0;
		public int systemTargetFrameRate = 60;
		public bool multiTouchEnabled = false;
		
		[Title("DOTween")]
		public int TweenersCapacity = 400;
		public int SequencesCapacity = 200;
	}
}