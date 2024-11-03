using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Global.UI.Loading;
using _Main.Scripts.Toolkit.Scene;
using Cysharp.Threading.Tasks;

namespace _Main.Scripts.Global.GameStates
{
    public class TransitSceneState : IParametrizedState<string>
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingScreen;
        private bool _isLoaded;
        private bool _idleAnimationIsFinished;

        public GameStateMachine.GameStateMachine StateMachine { get; set; }

        public TransitSceneState(ISceneLoader sceneLoader, LoadingScreen loadingScreen)
        {
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
            _sceneLoader.OnSceneLoaded += MarkFinishLoadScene;
            loadingScreen.OnIdleAnimationFinished += MarkFinishIdleAnimation;
        }

        public async UniTask Enter(string sceneName)
        {
            _isLoaded = false;
            _idleAnimationIsFinished = false;
            
            _loadingScreen.gameObject.SetActive(true);
            await _loadingScreen.PlayShowScreen();
            
            _sceneLoader.Load(sceneName, false);
            await UniTask.WaitUntil(() => _isLoaded && _idleAnimationIsFinished);
            _sceneLoader.LoadSceneOperation.allowSceneActivation = true;
        }

        public async UniTask Exit()
        {
            await _loadingScreen.PlayHideScreen();
            _loadingScreen.gameObject.SetActive(false);
        }

        private void MarkFinishLoadScene()
        {
            _isLoaded = true;
            _loadingScreen.StopIdleAnimationOnComplete();
        }

        private void MarkFinishIdleAnimation()
        {
            _idleAnimationIsFinished = true;
        }
    }
}