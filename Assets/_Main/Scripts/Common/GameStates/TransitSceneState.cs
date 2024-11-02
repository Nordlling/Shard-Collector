using Cysharp.Threading.Tasks;
using Main.Scripts.Infrastructure.States;

namespace _Main.Scripts.Common
{
    public class TransitSceneState : IParametrizedState<string>
    {
        private readonly ISceneLoader _sceneLoader;

        public GameStateMachine StateMachine { get; set; }

        public TransitSceneState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public async UniTask Enter(string sceneName)
        {
            // _serviceContainer.Get<CurtainUIView>().gameObject.SetActive(true);
            // await _serviceContainer.Get<CurtainUIView>().Enable();
            await _sceneLoader.Load(sceneName);
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}