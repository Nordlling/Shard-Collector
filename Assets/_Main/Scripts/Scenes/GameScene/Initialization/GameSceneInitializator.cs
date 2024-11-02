using System.Collections.Generic;
using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Scenes.GameScene.GameSceneStates;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Initialization
{
    public class GameSceneInitializator : IInitializable
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly List<IExitableState> _states;

        public GameSceneInitializator(IGameStateMachine gameStateMachine, List<IExitableState> states)
        {
            _gameStateMachine = gameStateMachine;
            _states = states;
        }

        public void Initialize()
        {
            foreach (var state in _states)
            {
                _gameStateMachine.AddState(state);
            }

            _gameStateMachine.Enter<StartGameSceneState>();
        }
    }
}