using CodeBase.Infrastructure.Events;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Food
{
    public class CopyClickedFood
    {
        private readonly EventReferer eventReferer;
        private readonly IFoodPool foodPool;

        [Inject]
        public CopyClickedFood(EventReferer eventReferer, IFoodPool foodPool)
        {
            this.eventReferer = eventReferer;
            this.foodPool = foodPool;
        }

        public void SubscribeToEvent() => 
            eventReferer.OnFoodClicked += CopyFood;

        public void UnSubscribeFromEvent() => 
            eventReferer.OnFoodClicked -= CopyFood;

        private async void CopyFood(GameObject food)
        {
            GameObject foodCopy = Object.Instantiate(food, food.transform.parent);
            foodPool.ChangeActiveObject(foodCopy, food);
            foodCopy.GetComponent<ClickFood>().Construct(eventReferer);
            foodCopy.SetActive(false);
            foodCopy.transform.position = new Vector3(0, -2, -.8f);
            await UniTask.Delay(500);
            foodCopy.SetActive(true);
            foodCopy.transform.DOLocalJump(Vector3.zero, .5f, 1, 1f);
        }
    }
}