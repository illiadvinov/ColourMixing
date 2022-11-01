using System;
using UnityEngine;

namespace CodeBase.Infrastructure.Events
{
    public class EventReferer
    {
        public event Action<GameObject> OnFoodClicked;
        public event Action OnLevelReset;
        public event Action OnMixedButtonClicked;
        public event Action<int> OnBlendingFinished;
        public event Action OnButtonClick;

        public void FoodClicked(GameObject gameObject) =>
            OnFoodClicked?.Invoke(gameObject);

        public void LevelReset() =>
            OnLevelReset?.Invoke();

        public void StartBlending() =>
            OnMixedButtonClicked?.Invoke();

        public void BlendingFinished(int blendResult) =>
            OnBlendingFinished?.Invoke(blendResult);

        public void ButtonClick() =>
            OnButtonClick?.Invoke();
    }
}