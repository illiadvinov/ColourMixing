using CodeBase.Blender;
using CodeBase.Character;
using CodeBase.Food;
using CodeBase.Infrastructure.Common.Factory;
using CodeBase.Infrastructure.Common.StateMachine;
using CodeBase.MixingLogic;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.GameStateMachine.States
{
    public class InitializeState : IState
    {
        private readonly IStateMachine stateMachine;
        private readonly IGameFactory<GameObject> foodFactory;
        private readonly IFoodPool foodPool;
        private readonly ICharacterPool charactersPool;
        private readonly MoveFoodToBlender moveFoodToBlender;
        private readonly CopyClickedFood copyClickedFood;
        private readonly Blending blending;
        private readonly HeadOpener headOpener;

        [Inject]
        public InitializeState(IStateMachine stateMachine,
            IFoodPool foodPool,
            ICharacterPool charactersPool,
            MoveFoodToBlender moveFoodToBlender,
            CopyClickedFood copyClickedFood,
            Blending blending,
            HeadOpener headOpener)
        {
            this.stateMachine = stateMachine;
            this.foodPool = foodPool;
            this.charactersPool = charactersPool;
            this.moveFoodToBlender = moveFoodToBlender;
            this.copyClickedFood = copyClickedFood;
            this.blending = blending;
            this.headOpener = headOpener;
        }

        public void Enter()
        {
            foodPool.Initialize();
            charactersPool.Initialize();
            EventSubscription();
            stateMachine.Enter<StartGameState>();
        }

        public void Exit()
        {
        }

        private void EventSubscription()
        {
            moveFoodToBlender.SubscribeToEvent();
            copyClickedFood.SubscribeToEvent();
            headOpener.SubscribeToEvent();
            //blending.SubscribeToEvent();
        }
    }
}