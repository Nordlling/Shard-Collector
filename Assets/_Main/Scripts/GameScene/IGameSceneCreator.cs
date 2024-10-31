namespace _Main.Scripts.GameScene
{
    public interface IGameSceneCreator
    {
        void CreateWorld();
        void Recreate();
        void DestroyWorld();
    }
}