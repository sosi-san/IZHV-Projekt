using System;
using System.Collections;
using UnityEngine;

namespace Woska.Core
{
    public class CoroutineHandle : IEnumerator
    {
        public Action Finished;
        public bool IsDone { get; private set; }
        public bool MoveNext() => !IsDone;
        public object Current { get; }

        public void Reset()
        {
        }
        public CoroutineHandle(MonoBehaviour owner, IEnumerator coroutine)
        {
            Current = owner.StartCoroutine(CoroutineWrapper(coroutine));
        }
        private IEnumerator CoroutineWrapper(IEnumerator coroutine )
        {
            yield return coroutine;
            IsDone = true;
            Finished?.Invoke();
        }
    }
}
