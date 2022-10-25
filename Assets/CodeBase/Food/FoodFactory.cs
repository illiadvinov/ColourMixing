using System.Collections.Generic;
using CodeBase.Infrastructure.Common.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase.Food
{
    public class FoodFactory : IGameFactory<GameObject[]>
    {
        private readonly Queue<GameObject> food;

        [Inject]
        public FoodFactory(IAssetProvider assetProvider)
        {
            food = new Queue<GameObject>(assetProvider.LoadObjects<GameObject>("Food/Prefabs"));
        }

        public GameObject[] CreateObject(Transform parent = null)
        {
            var foodCount = food.Count;
            GameObject[] foodInstances = new GameObject[foodCount];
            for (var i = 0; i < foodCount; i++)
            {
                var foodAsset = food.Dequeue();
                foodInstances[i] = Object.Instantiate(foodAsset, parent);
            }

            return foodInstances;
        }
    }
}