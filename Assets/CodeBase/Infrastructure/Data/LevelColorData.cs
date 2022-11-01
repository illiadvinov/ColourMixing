using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Data
{
    [CreateAssetMenu(fileName = "LevelColorData", menuName = "MainGame/LevelColor")]
    public class LevelColorData : ScriptableObject
    {
        public List<ColorFruits> colorFruitsList;
    }
}