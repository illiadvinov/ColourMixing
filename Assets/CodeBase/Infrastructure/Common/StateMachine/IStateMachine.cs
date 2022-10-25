namespace CodeBase.Infrastructure.Common.StateMachine
{
    public interface IStateMachine
    {
        void Enter<T>() where T : IState;
    }
}