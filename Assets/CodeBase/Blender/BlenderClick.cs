using CodeBase.Infrastructure.Data;
using CodeBase.Infrastructure.Events;
using UnityEngine;
using Zenject;

namespace CodeBase.Blender
{
    public class BlenderClick : MonoBehaviour
    {
        private EventReferer eventReferer;
        private LevelColorData levelColorData;

        [Inject]
        public void Construct(EventReferer eventReferer)
        {
            this.eventReferer = eventReferer;
            this.eventReferer.OnLevelReset += () => gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        private void OnMouseDown()
        {
            eventReferer.StartBlending();
            gameObject.SetActive(false);
        }
    }
}