using CodeBase.Food;
using UnityEngine;

namespace CodeBase.Character
{
    public interface ICharacterPool : IPool<GameObject>
    {
        GameObject GetActiveCharacter();
    }
}