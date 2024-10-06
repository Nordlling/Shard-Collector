using System;
namespace App.Scripts.Modules.Utils.RandomService
{
	public class UnityRandomService : IRandomService
	{
		public UnityRandomService(int seed)
		{
			UpdateSeed(seed);
		}

		public void UpdateSeed(int seed)
		{
			UnityEngine.Random.InitState(seed);
		}

		public int Range(int minValue, int maxValue)
		{
			return UnityEngine.Random.Range(minValue, maxValue);
		}

		public float Range(float minValue, float maxValue)
		{
			return UnityEngine.Random.Range(minValue, maxValue);
		}
		
		public bool RandomProbability(float chance, float fullChance) {
			var randomPoint = UnityEngine.Random.Range(0f, 1f) * fullChance;
			return chance >= randomPoint;
		}
	}
}