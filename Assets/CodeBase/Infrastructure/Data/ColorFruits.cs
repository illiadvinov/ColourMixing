using System;
using System.Collections.Generic;
using CodeBase.Food;
using UnityEngine;

namespace CodeBase.Infrastructure.Data
{
    [Serializable]
    public class ColorFruits
    {
        public Color color;
        public List<FoodIndex> foodIndexesToGetColor;
    }
}