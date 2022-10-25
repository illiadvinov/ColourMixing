using UnityEngine;

namespace CodeBase.Infrastructure.Common.Factory
{
    public interface IAssetProvider
    {
        T CreateObject<T>(string path) where T : Object;
        T[] LoadObjects<T>(string path) where T : Object;
    }
}