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

        private void CopyFood(GameObject food)
        {
            GameObject foodInPool = foodPool.GetSpecific(food.GetComponent<FoodInfoStorage>().Index);
            foodInPool.transform.SetParent(food.transform.parent);
            foodInPool.SetActive(true);
            foodInPool.transform.DOLocalJump(Vector3.zero, .5f, 1, 1f);
        }
    }
}