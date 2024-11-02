using Sirenix.OdinInspector;
using UnityEngine;

namespace _Main.Scripts.Global.Application.Configs
{
	[CreateAssetMenu(menuName = "Configs/Global/ApplicationSettingsConfig", fileName = "ApplicationSettingsConfig")]
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