using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Main.Scripts.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states = new();
        private IExitableState _activeState;

        public void AddState(IExitableState state)
        {
            _states[state.GetType()] = state;
            state.StateMachine = this;
        }
        
        public async UniTask Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            await state.Enter();
        }

        public async UniTask Enter<TState, TParameter>(TParameter param1) where TState : class, IParametrizedState<TParameter>
        {
            TState state = ChangeState<TState>();
            await state.Enter(param1);
        }

        public async UniTask Enter<TState, TParam1, TParam2>(TParam1 param1, TParam2 param2) where TState : class, IParametrizedState<TParam1, TParam2>
        {
            TState state = ChangeState<TState>();
            await state.Enter(param1, param2);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
      
            TState state = GetState<TState>();
            _activeState = state;
      
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}