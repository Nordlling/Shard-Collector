namespace _Main.Scripts.Toolkit.Random
{
	public class SystemRandomService : IRandomService
	{
		private System.Random _random;

		public SystemRandomService(int seed)
		{
			_random = new System.Random(seed);
			UnityEngine.Debug.Log("Seed = " + seed);
		}

		public void UpdateSeed(int seed)
		{
			_random = new System.Random(seed);
		}

		public int Range(int minValue, int maxValue)
		{
			return _random.Next(minValue, maxValue);
		}

		public float Range(float minValue, float maxValue)
		{
			var result = _random.NextDouble() * (maxValue - minValue) + minValue;
			return (float) result;
		}
		
		public bool RandomProbability(float chance, float fullChance) {
			var randomPoint = _random.NextDouble() * fullChance;
			return chance >= randomPoint;
		}
	}
}