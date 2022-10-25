using System.Collections.Generic;
using CodeBase.Infrastructure.Common.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase.Character
{
    public class CharacterFactory : IGameFactory<GameObject[]>
    {
        private readonly Queue<GameObject> characters;

        public CharacterFactory(IAssetProvider assetProvider) => 
            characters = new Queue<GameObject>(assetProvider.LoadObjects<GameObject>("Characters/Prefabs"));

        public GameObject[] CreateObject(Transform parent = null)
        {
            var charactersCount = characters.Count;
            GameObject[] characterInstances = new GameObject[charactersCount];
            for (var i = 0; i < charactersCount; i++)
            {
                var foodAsset = characters.Dequeue();
                characterInstances[i] = Object.Instantiate(foodAsset, parent);
            }

            return characterInstances;
        }
    }
}