using System.Collections;
using UnityEngine;

namespace Woska.Core
{
    public static class CoroutineExtensions
    {
        public static CoroutineHandle MyCoroutine(this MonoBehaviour self, IEnumerator coroutine)
        {
            return new CoroutineHandle(self, coroutine);
        }

    }
}
