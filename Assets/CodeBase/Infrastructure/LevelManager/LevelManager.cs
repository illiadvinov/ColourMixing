using System.Collections.Generic;
using CodeBase.Food;
using CodeBase.Infrastructure.Data;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.LevelManager
{
    public class LevelManager
    {
        private readonly LevelColorData levelColorData;
        private int level = 1;

        [Inject]
        public LevelManager(LevelColorData levelColorData) =>
            this.levelColorData = levelColorData;

        public Color GetNextLevelColor() =>
            levelColorData.colorFruitsList[level].color;

        public List<FoodIndex> GetTrueFood =>
            levelColorData.colorFruitsList[level].foodIndexesToGetColor;

        public void NextLevel() =>
            level++;
    }
}