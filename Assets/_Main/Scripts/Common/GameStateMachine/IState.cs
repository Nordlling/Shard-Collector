using Cysharp.Threading.Tasks;

namespace Main.Scripts.Infrastructure.States
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
    
    public interface IParametrizedState<TParam1> : IExitableState
    {
        UniTask Enter(TParam1 param);
    }
    
    public interface IParametrizedState<TParam1, TParam2> : IExitableState
    {
        UniTask Enter(TParam1 param1, TParam2 param2);
    }

    public interface IExitableState
    {
        UniTask Exit();
        
        GameStateMachine StateMachine { get; set; }
    }
}