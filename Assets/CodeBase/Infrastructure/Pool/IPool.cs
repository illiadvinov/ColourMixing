using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Food
{
    public interface IPool<T>
    {
        void Initialize();
        void Add(T t);
        T GetRandom();
        T GetSpecific(int index);
    }

    public interface IFoodPool : IPool<GameObject>
    {
        List<GameObject> activeObjects { get; }
        void ChangeActiveObject(GameObject activeObject, GameObject poolObject);
       // GameObject GetActiveFood();
    }
}