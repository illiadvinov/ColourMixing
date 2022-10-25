using UnityEngine;

namespace CodeBase.Infrastructure.Common.Factory
{
    public class AssetProvider : IAssetProvider
    {
        public T CreateObject<T>(string path) where T : Object
        {
            T objectInstance = Resources.Load<T>(path);
            return Object.Instantiate(objectInstance);
        }

        public T[] LoadObjects<T>(string path) where T : Object => 
            Resources.LoadAll<T>(path);
    }
}