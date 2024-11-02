using Cysharp.Threading.Tasks;

namespace _Main.Scripts.Global.GameStateMachine
{
    public interface IGameStateMachine
    {
        void AddState(IExitableState state);
        
        UniTask Enter<TState>() where TState : class, IState;
        UniTask Enter<TState, TParam1>(TParam1 param1) where TState : class, IParametrizedState<TParam1>;
        UniTask Enter<TState, TParam1, TParam2>(TParam1 param1, TParam2 param2) where TState : class, IParametrizedState<TParam1, TParam2>;
    }
}