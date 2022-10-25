namespace CodeBase.Infrastructure.Common.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit();
    }
}