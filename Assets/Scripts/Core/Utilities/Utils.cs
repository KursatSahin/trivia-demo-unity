using UnityEngine;

namespace Core.Utils
{
    public static class MUtils
    {
        public static bool HasComponent<T> (this GameObject obj)
        {
            return obj.GetComponent(typeof(T)) != null;
        }
    }
}