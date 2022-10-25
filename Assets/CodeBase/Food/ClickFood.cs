using CodeBase.Infrastructure.Events;
using UnityEngine;

namespace CodeBase.Food
{
    public class ClickFood : MonoBehaviour
    {
        private EventReferer eventReferer;

        public void Construct(EventReferer eventReferer) =>
            this.eventReferer = eventReferer;

        private void OnMouseDown() =>
            eventReferer.FoodClicked(gameObject);
    }
}