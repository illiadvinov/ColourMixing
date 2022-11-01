using System;
using CodeBase.Infrastructure.Events;
using UnityEngine;
using Zenject;

namespace CodeBase.Blender
{
    public class MixPanelColorSet
    {
        private Material material;
        private EventReferer eventReferer;

        [Inject]
        public MixPanelColorSet(EventReferer eventReferer, [Inject(Id = "MixPanel")] GameObject mixPanel)
        {
            this.eventReferer = eventReferer;
            material = mixPanel.GetComponent<Renderer>().material;
        }

        public void SubscribeToEvent()
        {
            eventReferer.OnMixedButtonClicked += SetColorToRed;
            eventReferer.OnLevelReset += SetColorToGreen;
        }

        public void UnsubscribeFromEvent()
        {
            eventReferer.OnMixedButtonClicked -= SetColorToRed;
            eventReferer.OnLevelReset -= SetColorToGreen;
        }

        private void SetColorToGreen() =>
            SetColorAndEmission(Color.green);

        private void SetColorToRed() =>
            SetColorAndEmission(Color.red);

        private void SetColorAndEmission(Color color)
        {
            material.color = color;
            material.SetColor("_EmissionColor", color);
        }
    }
}