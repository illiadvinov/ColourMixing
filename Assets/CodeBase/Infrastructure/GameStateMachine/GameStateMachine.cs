using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Common.StateMachine;
using CodeBase.Infrastructure.GameStateMachine.States;
using Zenject;

namespace CodeBase.Infrastructure.GameStateMachine
{
    public class StateMachine : IStateMachine
    {
        private Dictionary<Type, IState> states;
        private IState previousState;

        [Inject]
        public void Construct([Inject(Id = "InitializeState")] IState initialize,
            [Inject(Id = "StartGameState")] IState start,
            [Inject(Id = "GamePlayState")] IState gamePlay,
            [Inject(Id = "NextLevelState")] IState nextLevel)
        {
            states = new Dictionary<Type, IState>
            {
                [typeof(InitializeState)] = initialize,
                [typeof(StartGameState)] = start,
                [typeof(GamePlayState)] = gamePlay,
                [typeof(NextLevelState)] = nextLevel,
            };
        }

        public void Enter<TState>() where TState : IState
        {
            previousState?.Exit();
            IState state = states[typeof(TState)];
            previousState = state;
            state.Enter();
        }
    }
}