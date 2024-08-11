using UnityEngine;

namespace Soul.Model.Runtime.Coroutines
{
    public class CoroutineLink<T>
    {
        public Coroutine Coroutine;
        public T Link;
    }
}