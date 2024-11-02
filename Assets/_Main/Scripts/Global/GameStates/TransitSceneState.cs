using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Toolkit.Scene;
using Cysharp.Threading.Tasks;

namespace _Main.Scripts.Global.GameStates
{
    public class TransitSceneState : IParametrizedState<string>
    {
        private readonly ISceneLoader _sceneLoader;

        public GameStateMachine.GameStateMachine StateMachine { get; set; }

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