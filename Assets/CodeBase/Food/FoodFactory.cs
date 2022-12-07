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

        // public GameObject[] CreateObject(Transform parent = null)
        // {
        //     var foodCount = food.Count;
        //     GameObject[] foodInstances = new GameObject[50];
        //     var foodAsset = food.Dequeue();
        //     for (var i = 0; i < 50; i++)
        //     {
        //         foodInstances[i] = Object.Instantiate(foodAsset, parent);
        //     }
        //
        //     return foodInstances;
        // }

        public GameObject[] CreateObject(Transform parent = null)
        {
            var foodCount = food.Count;
            GameObject[] foodInstances = new GameObject[foodCount];
            for (var i = 0; i < foodCount; i++)
            {
                var foodAsset = food.Dequeue();
                foodInstances[i] = Object.Instantiate(foodAsset, parent);
                foodInstances[i].name = foodInstances[i].name.Replace("(Clone)", "").Trim();
            }

            return foodInstances;
        }
    }

    // public interface IFoodFactory<T, K> : IGameFactory<T>
    // {
    //     public void CreateB
    // }
}