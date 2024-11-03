using System.Collections.Generic;
using _Main.Scripts.Global.GameStateMachine;
using Zenject;

namespace _Main.Scripts.Global.Initialization
{
    public class CommonInitializator : IInitializable
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly List<IExitableState> _states;

        public CommonInitializator(IGameStateMachine gameStateMachine, List<IExitableState> states)
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
        }
    }
}