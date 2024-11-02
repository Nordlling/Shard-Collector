using System.Collections.Generic;
using Main.Scripts.Infrastructure.States;
using Zenject;

namespace _Main.Scripts.Common.Installers
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