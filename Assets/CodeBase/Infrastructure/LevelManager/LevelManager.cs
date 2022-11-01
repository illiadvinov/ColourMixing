using System.Collections.Generic;
using CodeBase.Food;
using CodeBase.Infrastructure.Data;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.LevelManager
{
    public class LevelManager
    {
        private const int MinForNextLevel = 85;

        private readonly LevelColorData levelColorData;
        private readonly GameObject colliderCurtain;
        private readonly ButtonActivator buttonActivator;
        private int level = 1;

        [Inject]
        public LevelManager(LevelColorData levelColorData,
            [Inject(Id = "ColliderCurtain")] GameObject colliderCurtain,
            ButtonActivator buttonActivator)
        {
            this.levelColorData = levelColorData;
            this.colliderCurtain = colliderCurtain;
            this.buttonActivator = buttonActivator;
        }

        public Color GetNextLevelColor() =>
            levelColorData.colorFruitsList[level].color;

        public List<FoodIndex> GetTrueFood =>
            levelColorData.colorFruitsList[level].foodIndexesToGetColor;

        public void NextLevel() =>
            level++;

        public bool CheckResult(int blendResult)
        {
            colliderCurtain.SetActive(true);
            if (blendResult >= MinForNextLevel)
            {
                buttonActivator.ActivateNextButton(blendResult);
                return true;
            }

            buttonActivator.ActivateRestartButton(blendResult);
            return false;
        }
    }
}