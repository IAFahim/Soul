using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Coroutines
{
    public class CoroutineLink<T>
    {
        public Coroutine Coroutine;
        public T Link;
    }
}