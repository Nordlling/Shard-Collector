using _Main.Scripts.Global.Ecs.World;
using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Toolkit.InputSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.GameSceneStates
{
    public class PlayLevelState : IState, ITickable
    {
        public GameStateMachine StateMachine { get; set; }
        
        private readonly IInputService _inputService;
        private readonly IWorldRunner _worldRunner;

        public PlayLevelState(IInputService inputService, IWorldRunner worldRunner)
        {
            _inputService = inputService;
            _worldRunner = worldRunner;
        }

        public UniTask Enter()
        {
            _inputService.EnableInput();
            _inputService.EnableUIInput();
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
        
        public void Tick()
        {
            _worldRunner.Update(Time.deltaTime);
        }
        
    }
}