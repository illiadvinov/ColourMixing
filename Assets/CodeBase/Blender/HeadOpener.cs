using System.Threading;
using CodeBase.Infrastructure.Events;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Blender
{
    public class HeadOpener
    {
        private bool isOpen;
        private bool animationIsFinished;
        private readonly EventReferer eventReferer;
        private readonly GameObject blenderHead;

        [Inject]
        public HeadOpener(
            EventReferer eventReferer,
            [Inject(Id = "BlenderHead")] GameObject blenderHead)
        {
            animationIsFinished = false;
            isOpen = false;
            this.eventReferer = eventReferer;
            this.blenderHead = blenderHead;
        }

        public void SubscribeToEvent()
        {
            eventReferer.OnFoodClicked += MoveHead;
            eventReferer.OnMixedButtonClicked += CloseHead;
        }

        private async void MoveHead(GameObject food)
        {
            var c = new CancellationToken();
            if (isOpen)
                return;
            isOpen = true;
            blenderHead.transform.DOLocalMoveX(.18f, .5f);
            await UniTask.Delay(4500, cancellationToken: c);
            if (c.IsCancellationRequested)
                return;

            isOpen = false;
            if (!isOpen)
                CloseHead();
        }

        private void CloseHead()
        {
            blenderHead.transform.DOLocalMoveX(0, .5f);
            isOpen = false;
        }
    }
}