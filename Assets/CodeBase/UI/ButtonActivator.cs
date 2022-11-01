using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class ButtonActivator
    {
        private readonly GameObject nextButton;
        private readonly GameObject restartButton;
        private readonly TMP_Text nextButtonTextResult;
        private readonly TMP_Text restartButtonTextResult;

        [Inject]
        public ButtonActivator([Inject(Id = "ButtonsContainer")] Transform buttonsContainer)
        {
            nextButton = buttonsContainer.GetChild(0).gameObject;
            restartButton = buttonsContainer.GetChild(1).gameObject;
            nextButtonTextResult = nextButton.transform.GetChild(0).GetComponent<TMP_Text>();
            restartButtonTextResult = restartButton.transform.GetChild(0).GetComponent<TMP_Text>();
        }

        public void ActivateNextButton(int blendResult)
        {
            nextButton.SetActive(true);
            nextButtonTextResult.SetText(blendResult.ToString());
            nextButton.transform.DOScale(new Vector3(7.6f, 7.6f, 7.6f), 1f);
        }

        public void DeactivateNextButton()
        {
            nextButton.SetActive(false);
            nextButton.transform.DOScale(Vector3.zero, 1f);
        }

        public void ActivateRestartButton(int blendResult)
        {
            restartButton.SetActive(true);
            restartButtonTextResult.SetText(blendResult.ToString());
            restartButton.transform.DOScale(new Vector3(7.6f, 7.6f, 7.6f), 1f);
        }

        public void DeactivateRestartButton()
        {
            restartButton.SetActive(false);
            restartButton.transform.DOScale(Vector3.zero, 1f);
        }
    }
}