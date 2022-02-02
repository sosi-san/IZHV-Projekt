using UnityEngine;

namespace Woska.Core
{
    public static class TransformExtensions
    {
        public static void MoveTowards(this Transform self, Transform target, float speed)
        {
            self.position = Vector3.MoveTowards(self.position, target.position, speed * Time.deltaTime);
        }
    }
}
