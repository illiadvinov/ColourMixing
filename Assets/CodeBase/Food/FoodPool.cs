using System.Collections.Generic;
using CodeBase.Infrastructure.Common.Factory;
using CodeBase.Infrastructure.Events;
using UnityEngine;
using Zenject;

namespace CodeBase.Food
{
    public class FoodPool : IFoodPool
    {
        public List<GameObject> activeObjects { get; }
        private readonly Transform foodContainer;
        private readonly EventReferer eventReferer;
        private readonly IGameFactory<GameObject[]> foodFactory;
        private List<GameObject> objectsInPool;

        [Inject]
        public FoodPool([Inject(Id = "FoodContainer")] Transform foodContainer,
            IAssetProvider assetProvider,
            EventReferer eventReferer)
        {
            this.foodContainer = foodContainer;
            this.eventReferer = eventReferer;
            foodFactory = new FoodFactory(assetProvider);
            objectsInPool = new List<GameObject>();
            activeObjects = new List<GameObject>();
        }


        public void Initialize()
        {
            int indexForFood = 0;

            foreach (var foodInstance in foodFactory.CreateObject(foodContainer))
            {
                Add(foodInstance);
                foodInstance.SetActive(true);
                foodInstance.GetComponent<FoodInfoStorage>().Index = indexForFood;
                foodInstance.AddComponent<ClickFood>().Construct(eventReferer);
                indexForFood++;
            }
        }

        public void Add(GameObject gameObject)
        {
            gameObject.transform.SetParent(foodContainer);
            gameObject.transform.position = Vector3.zero;

            objectsInPool.Add(gameObject);
            if (activeObjects.Contains(gameObject))
                activeObjects.Remove(gameObject);
        }

        public GameObject GetRandom()
        {
            if (objectsInPool.Capacity <= 0)
                return null;

            var foodIndex = Random.Range(0, objectsInPool.Count);
            GameObject tempFood = objectsInPool[foodIndex];
            objectsInPool.Remove(tempFood);
            activeObjects.Add(tempFood);

            return tempFood;
        }

        public GameObject GetSpecific(int foodIndex)
        {
            foreach (GameObject food in objectsInPool)
            {
                if (food.GetComponent<FoodInfoStorage>().Index == foodIndex)
                {
                    objectsInPool.Remove(food);
                    activeObjects.Add(food);
                    return food;
                }
            }

            return null;
        }

        public void ChangeActiveObject(GameObject activeObject, GameObject poolObject)
        {
            if (activeObjects.Contains(poolObject))
                activeObjects[activeObjects.IndexOf(poolObject)] = activeObject;
        }

        // public GameObject GetActiveFood()
        // {
        //     var activeObject = activeObjects[0];
        //     
        // }
    }
}