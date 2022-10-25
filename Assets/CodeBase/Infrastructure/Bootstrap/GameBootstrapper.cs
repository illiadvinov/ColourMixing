using CodeBase.Infrastructure.Common.StateMachine;
using CodeBase.Infrastructure.GameStateMachine.States;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Bootstrap
{
    public class GameBootstrapper : MonoBehaviour
    {
        private IStateMachine stateMachine;

        [Inject]
        public void Construct(IStateMachine stateMachine) =>
            this.stateMachine = stateMachine;

        private void Awake() =>
            stateMachine.Enter<InitializeState>();
    }
}