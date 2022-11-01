using System.Collections.Generic;
using CodeBase.Food;
using CodeBase.Infrastructure.Common.StateMachine;
using CodeBase.Infrastructure.Events;
using CodeBase.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.GameStateMachine.States
{
    public class GamePlayState : IState
    {
        private readonly MoveFoodToBlender moveFoodToBlender;
        private readonly IStateMachine stateMachine;
        private readonly LevelManager.LevelManager levelManager;
        private readonly FoodSetUp foodSetUp;
        private readonly ButtonActivator buttonActivator;
        private readonly ClickFood clickFood;
        private readonly EventReferer eventReferer;
        private readonly GameObject colliderCurtain;
        private readonly List<Transform> foodPositionsOnTable;

        private bool isNextButton;
        private readonly GameObject nextButton;
        private readonly GameObject restartButton;
        private readonly TMP_Text nextButtonTextResult;
        private readonly TMP_Text restartButtonTextResult;

        [Inject]
        public GamePlayState(MoveFoodToBlender moveFoodToBlender,
            EventReferer eventReferer,
            IStateMachine stateMachine,
            LevelManager.LevelManager levelManager,
            FoodSetUp foodSetUp,
            ButtonActivator buttonActivator,
            [Inject(Id = "ColliderCurtain")] GameObject colliderCurtain)
        {
            this.moveFoodToBlender = moveFoodToBlender;
            this.stateMachine = stateMachine;
            this.levelManager = levelManager;
            this.foodSetUp = foodSetUp;
            this.buttonActivator = buttonActivator;
            eventReferer.OnBlendingFinished += ResetFoodOnTable;
            eventReferer.OnButtonClick += ButtonClick;
            this.colliderCurtain = colliderCurtain;
            this.eventReferer = eventReferer;
        }

        public void Enter()
        {
            colliderCurtain.SetActive(false);
            eventReferer.LevelReset();
            foodSetUp.SetUpActiveFoodOnScene();
        }

        public void Exit()
        {
        }


        private void ResetFoodOnTable(int index)
        {
            moveFoodToBlender.PrepareForNextLevel();
            foodSetUp.ResetFoodOnTable(() => isNextButton = levelManager.CheckResult(index));
        }


        private void ButtonClick()
        {
            if (isNextButton)
            {
                buttonActivator.DeactivateNextButton();
                levelManager.NextLevel();
                stateMachine.Enter<NextLevelState>();
            }
            else
            {
                buttonActivator.DeactivateRestartButton();
                stateMachine.Enter<GamePlayState>();
            }
        }
    }
}