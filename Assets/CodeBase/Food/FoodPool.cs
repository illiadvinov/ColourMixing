using System.Collections.Generic;
using CodeBase.Infrastructure.Common.Factory;
using CodeBase.Infrastructure.Events;
using CodeBase.Infrastructure.Pool;
using UnityEngine;
using Zenject;

namespace CodeBase.Food
{
    public class FoodPool : IFoodPool
    {
        private const int TimeToRepeatStorage = 10;

        private List<GameObject> activeObjects { get; }
        private readonly Transform foodContainer;
        private readonly EventReferer eventReferer;
        private readonly IGameFactory<GameObject[]> foodFactory;
        private GameObject[] foodInstances;
        private Stack<GameObject>[] objectsInPool;


        [Inject]
        public FoodPool([Inject(Id = "FoodContainer")] Transform foodContainer,
            IAssetProvider assetProvider,
            EventReferer eventReferer)
        {
            this.foodContainer = foodContainer;
            this.eventReferer = eventReferer;
            foodFactory = new FoodFactory(assetProvider);
            activeObjects = new List<GameObject>();
        }


        public void Initialize()
        {
            foodInstances = foodFactory.CreateObject(foodContainer);
            objectsInPool = new Stack<GameObject>[foodInstances.Length];
            InitializeFoodPool();
        }

        public void Add(GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(foodContainer);
            gameObject.transform.position = Vector3.zero;

            objectsInPool[gameObject.GetComponent<FoodInfoStorage>().Index].Push(gameObject);
            if (activeObjects.Contains(gameObject))
                activeObjects.Remove(gameObject);
        }

        public GameObject GetSpecific(int foodIndex)
        {
            if (objectsInPool[foodIndex].Count <= 0)
                RefillSpecificFood(foodIndex);

            GameObject food = objectsInPool[foodIndex].Pop();
            activeObjects.Add(food);
            food.SetActive(true);
            return food;
        }

        public GameObject GetRandom()
        {
            if (objectsInPool.Length <= 0)
                InitializeFoodPool();

            int foodIndex = Random.Range(0, objectsInPool.Length);
            return GetSpecific(foodIndex);
        }

        private void InitializeFoodPool()
        {
            int indexForFood = 0;
            for (int i = 0; i < foodInstances.Length; i++)
            {
                GameObject food = foodInstances[i];
                objectsInPool[i] ??= new Stack<GameObject>();

                for (int j = 0; j < TimeToRepeatStorage; j++)
                    CopyFood(food, indexForFood);

                indexForFood++;
            }
        }

        private void RefillSpecificFood(int index)
        {
            for (int i = 0; i < TimeToRepeatStorage; i++)
                CopyFood(foodInstances[index], index);
        }

        private void CopyFood(GameObject foodReference, int indexForFood)
        {
            GameObject foodInstance = Object.Instantiate(foodReference, foodContainer);
            foodInstance.name = foodInstance.name.Replace("(Clone)", "").Trim();

            foodInstance.GetComponent<FoodInfoStorage>().Index = indexForFood;
            foodInstance.AddComponent<ClickFood>().Construct(eventReferer);
            Add(foodInstance);
        }
    }
}