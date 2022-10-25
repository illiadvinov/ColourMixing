using System.Collections.Generic;
using CodeBase.Food;
using CodeBase.Infrastructure.Common.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase.Character
{
    public class CharacterPool : ICharacterPool
    {
        private readonly Transform charactersContainer;
        private readonly IAssetProvider assetProvider;
        private readonly IGameFactory<GameObject[]> characterFactory;
        private List<GameObject> characterPool;
        private GameObject activeCharacter;

        [Inject]
        public CharacterPool(IAssetProvider assetProvider,
            [Inject(Id = "CharacterContainer")] Transform charactersContainer)
        {
            this.charactersContainer = charactersContainer;
            characterFactory = new CharacterFactory(assetProvider);
            characterPool = new List<GameObject>();
        }

        public void Initialize()
        {
            foreach (GameObject characters in characterFactory.CreateObject(charactersContainer))
            {
                Add(characters);
                characters.AddComponent<CharacterAnimations>();
            }
        }

        public void Add(GameObject t)
        {
            characterPool.Add(t);
            t.SetActive(false);
        }

        public GameObject GetRandom()
        {
            GameObject character = characterPool[Random.Range(0, characterPool.Count)];
            characterPool.Remove(character);
            activeCharacter = character;
            character.SetActive(true);
            return character;
        }

        public GameObject GetSpecific(int index)
        {
            if (characterPool.Contains(characterPool[index]))
            {
                GameObject character = characterPool[index];
                characterPool.Remove(character);
                return character;
            }

            return null;
        }

        public GameObject GetActiveCharacter() =>
            activeCharacter;
    }
}