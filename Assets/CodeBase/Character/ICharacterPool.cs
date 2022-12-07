using CodeBase.Food;
using CodeBase.Infrastructure.Pool;
using UnityEngine;

namespace CodeBase.Character
{
    public interface ICharacterPool : IPool<GameObject>
    {
        GameObject GetActiveCharacter();
    }
}