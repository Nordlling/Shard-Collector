namespace App.Scripts.Modules.Utils.RandomService
{
	public interface IRandomService
	{
		void UpdateSeed(int seed);
		int Range(int minValue, int maxValue);
		float Range(float minValue, float maxValue);
		bool RandomProbability(float chance, float fullChance);
	}
}