using System;
using System.Collections.Generic;
using CodeBase.Food;
using UnityEngine;

namespace CodeBase.Infrastructure.Data
{
    [CreateAssetMenu(fileName = "LevelColorData", menuName = "MainGame/LevelColor")]
    public class LevelColorData : ScriptableObject
    {
        public List<ColorFruits> colorFruitsList;
    }

    [Serializable]
    public class ColorFruits
    {
        public Color color;
        public List<FoodIndex> foodIndexesToGetColor;
    } 
}