using System.Collections.Generic;
using CodeBase.Blender;
using CodeBase.Food;
using CodeBase.Infrastructure.Common.StateMachine;
using CodeBase.Infrastructure.Events;
using CodeBase.MixingLogic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.GameStateMachine.States
{
    public class GamePlayState : IState
    {
        private const int MinForNextLevel = 85;

        private readonly IFoodPool foodPool;
        private readonly MoveFoodToBlender moveFoodToBlender;
        private readonly Blending blending;
        private readonly IStateMachine stateMachine;
        private readonly LevelManager.LevelManager levelManager;
        private readonly MixPanelColorSet mixPanelColorSet;
        private readonly BlenderShaking blenderShaking;
        private readonly ClickFood clickFood;
        private readonly EventReferer eventReferer;
        private readonly Transform foodPosition;
        private readonly GameObject colliderCurtain;
        private readonly List<Transform> foodPositionsOnTable;

        private bool isNextButton;
        private readonly GameObject nextButton;
        private readonly GameObject restartButton;
        private readonly TMP_Text nextButtonTextResult;
        private readonly TMP_Text restartButtonTextResult;

        [Inject]
        public GamePlayState(IFoodPool foodPool, MoveFoodToBlender moveFoodToBlender, Blending blending,
            EventReferer eventReferer,
            IStateMachine stateMachine,
            LevelManager.LevelManager levelManager,
            MixPanelColorSet mixPanelColorSet,
            BlenderShaking blenderShaking,
            [Inject(Id = "FoodPosition")] Transform foodPosition,
            [Inject(Id = "ButtonsContainer")] Transform buttonsContainer,
            [Inject(Id = "ColliderCurtain")] GameObject colliderCurtain)
        {
            this.foodPool = foodPool;
            this.moveFoodToBlender = moveFoodToBlender;
            this.blending = blending;
            this.stateMachine = stateMachine;
            this.levelManager = levelManager;
            this.mixPanelColorSet = mixPanelColorSet;
            this.blenderShaking = blenderShaking;
            eventReferer.OnBlendingFinished += ResetFoodOnTable;
            eventReferer.OnButtonClick += ButtonClick;
            this.foodPosition = foodPosition;
            this.colliderCurtain = colliderCurtain;
            this.eventReferer = eventReferer;


            foodPositionsOnTable = new List<Transform>();
            for (int i = 0; i < foodPosition.childCount; i++)
                foodPositionsOnTable.Add(foodPosition.GetChild(i));

            nextButton = buttonsContainer.GetChild(0).gameObject;
            restartButton = buttonsContainer.GetChild(1).gameObject;
            nextButtonTextResult = nextButton.transform.GetChild(0).GetComponent<TMP_Text>();
            restartButtonTextResult = restartButton.transform.GetChild(0).GetComponent<TMP_Text>();
        }

        public void Enter()
        {
            colliderCurtain.SetActive(false);
            eventReferer.LevelReset();
            SetUpActiveFoodOnScene();
        }

        public void Exit()
        {
        }

        private void SetUpActiveFoodOnScene()
        {
            List<GameObject> foodOnScene = new();
            foreach (FoodIndex foodIndex in levelManager.GetTrueFood)
                foodOnScene.Add(foodPool.GetSpecific((int) foodIndex));

            for (int i = 0; i <= foodPositionsOnTable.Count - foodOnScene.Count; i++)
                foodOnScene.Add(foodPool.GetRandom());

            List<Transform> tempFoodPosition = new(foodPositionsOnTable);
            foreach (GameObject food in foodOnScene)
            {
                Transform foodPositionOnTable = tempFoodPosition[Random.Range(0, tempFoodPosition.Count)];
                food.transform.SetParent(foodPositionOnTable);
                tempFoodPosition.Remove(foodPositionOnTable);
                food.gameObject.SetActive(true);
                food.transform.DOLocalJump(Vector3.zero, .5f, 1, 2f);
            }
        }


        private void ResetFoodOnTable(int index)
        {
            moveFoodToBlender.PrepareForNextLevel();

            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < foodPosition.childCount; i++)
            {
                var food = foodPosition.GetChild(i).GetChild(0);
                sequence.Join(food.transform
                    .DOLocalJump(new Vector3(0, 0, -2.5f), 1f, 1, 2));
            }

            sequence.OnComplete(() =>
            {
                for (int i = 0; i < foodPosition.childCount; i++)
                {
                    var food = foodPosition.GetChild(i).GetChild(0);
                    foodPool.Add(food.gameObject);
                    food.gameObject.SetActive(false);
                }

                CheckResult(index);
            });
        }

        private void CheckResult(int blendResult)
        {
            colliderCurtain.SetActive(true);
            if (blendResult >= MinForNextLevel)
            {
                ActivateButton(nextButton, nextButtonTextResult, blendResult);
                isNextButton = true;
            }
            else
            {
                ActivateButton(restartButton, restartButtonTextResult, blendResult);
                isNextButton = false;
            }
        }

        private void ButtonClick()
        {
            if (isNextButton)
            {
                DeactivateButton(nextButton);
                levelManager.NextLevel();
                stateMachine.Enter<NextLevelState>();
            }
            else
            {
                DeactivateButton(restartButton);
                stateMachine.Enter<GamePlayState>();
            }
        }

        private void ActivateButton(GameObject gameObject, TMP_Text buttonTextResult, int blendResult)
        {
            gameObject.SetActive(true);
            buttonTextResult.SetText(blendResult.ToString());
            gameObject.transform.DOScale(new Vector3(7.6f, 7.6f, 7.6f), 1f);
        }

        private void DeactivateButton(GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.DOScale(Vector3.zero, 1f);
        }
    }
}