using UnityEngine;

namespace CodeBase.Infrastructure.Common.Factory
{
    public interface IGameFactory<T>
    {
        T CreateObject(Transform parent = null);
    }
}