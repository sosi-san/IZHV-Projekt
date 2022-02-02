using UnityEngine;

namespace Woska.Core
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            return self.GetComponent<T>() ?? self.AddComponent<T>();
        }
        public static bool HasComponent<T>(this GameObject self) where T : Component
        {
            return self.GetComponent<T>() != null;
        }
    }
}
