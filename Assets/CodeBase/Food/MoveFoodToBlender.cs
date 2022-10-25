using System.Collections.Generic;
using CodeBase.Infrastructure.Events;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Food
{
    public class MoveFoodToBlender
    {
        private const uint maxAmountOfFood = 7;

        public List<GameObject> foodList;
        public List<GameObject> unfitFoodList;

        private readonly EventReferer eventReferer;
        private readonly Transform foodInBlenderPosition;

        [Inject]
        public MoveFoodToBlender(EventReferer eventReferer, [Inject(Id = "FoodInBlender")] Transform foodInBlenderPosition)
        {
            this.eventReferer = eventReferer;
            this.foodInBlenderPosition = foodInBlenderPosition;
            foodList = new List<GameObject>();
            unfitFoodList = new List<GameObject>();
        }

        public void SubscribeToEvent() =>
            eventReferer.OnFoodClicked += ClickReaction;

        public void UnSubscribeFromEvent() =>
            eventReferer.OnFoodClicked -= ClickReaction;

        public void PrepareForNextLevel()
        {
            DestroyFoodInBlender();
            unfitFoodList.Clear();
        }

        public void DestroyFoodInBlender()
        {
            foreach (GameObject food in foodList)
            {
                food.SetActive(false);
                Object.Destroy(food);
            }

            foodList.Clear();
        }

        private async void ClickReaction(GameObject food)
        {
            await UniTask.Delay(100);
            if (foodList.Count > maxAmountOfFood)
            {
                FoodAnimationToBlender(food);

                await UniTask.Delay(5000);
                food.SetActive(false);
                unfitFoodList.Add(food);
                return;
            }

            foodList.Add(food);
            FoodAnimationToBlender(food);
        }

        private void FoodAnimationToBlender(GameObject food)
        {
            food.transform.SetParent(foodInBlenderPosition);
            food.transform.DOLocalJump(Vector3.zero, .2f, 1, 2f).SetEase(Ease.OutSine).OnComplete(() => food.GetComponent<Rigidbody>().isKinematic = false);
        }
    }
}