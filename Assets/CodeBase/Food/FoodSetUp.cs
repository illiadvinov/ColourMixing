using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.LevelManager;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Food
{
    public class FoodSetUp
    {
        private readonly IFoodPool foodPool;
        private readonly LevelManager levelManager;
        private readonly MoveFoodToBlender moveFoodToBlender;
        private readonly List<Transform> foodPositionsOnTable;

        [Inject]
        public FoodSetUp(IFoodPool foodPool, LevelManager levelManager, MoveFoodToBlender moveFoodToBlender,
            [Inject(Id = "FoodPosition")] Transform foodPosition)
        {
            this.foodPool = foodPool;
            this.levelManager = levelManager;
            this.moveFoodToBlender = moveFoodToBlender;

            foodPositionsOnTable = new List<Transform>();
            for (int i = 0; i < foodPosition.childCount; i++)
                foodPositionsOnTable.Add(foodPosition.GetChild(i));
        }

        public void SetUpActiveFoodOnScene()
        {
            List<GameObject> foodOnScene = levelManager.GetTrueFood
                .Select(foodIndex => foodPool.GetSpecific((int) foodIndex)).ToList();

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

        public void ResetFoodOnTable(Action callback)
        {
            moveFoodToBlender.PrepareForNextLevel();

            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < foodPositionsOnTable.Count; i++)
            {
                var food = foodPositionsOnTable[i].GetChild(0);
                sequence.Join(food.transform
                    .DOLocalJump(new Vector3(0, 0, -2.5f), 1f, 1, 2));
            }

            sequence.OnComplete(() =>
            {
                foreach (Transform foodPositionOnTable in foodPositionsOnTable)
                {
                    Transform food = foodPositionOnTable.GetChild(0);
                    foodPool.Add(food.gameObject);
                    food.gameObject.SetActive(false);
                }

                callback.Invoke();
            });
        }
    }
}