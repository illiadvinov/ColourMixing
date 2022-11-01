using CodeBase.Infrastructure.Events;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Blender
{
    public class BlenderShaking
    {
        private readonly EventReferer eventReferer;
        private readonly Animator animator;
        private static readonly int IsShaking = Animator.StringToHash("isShaking");

        [Inject]
        public BlenderShaking(EventReferer eventReferer,
            [Inject(Id = "Blender")] GameObject blender)
        {
            this.eventReferer = eventReferer;
            animator = blender.GetComponent<Animator>();
        }

        public void SubscribeToEvent() =>
            eventReferer.OnMixedButtonClicked += StartShakingAnimation;

        public void UnsubscribeFromEvent() =>
            eventReferer.OnMixedButtonClicked -= StartShakingAnimation;

        private async void StartShakingAnimation()
        {
            animator.SetBool(IsShaking, true);
            await UniTask.Delay(2000);
            animator.SetBool(IsShaking, false);
        }
    }
}