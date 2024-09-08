using Alchemy.Inspector;
using Pancake;
using Pancake.Pools;
using Soul.Model.Runtime.Extensions;

namespace Soul.Model.Runtime.PoolAbles
{
    public class PoolAbleComponent : GameComponent, IPoolCallbackReceiver
    {
        [Title("PoolAbleComponent")]
        protected bool FromPool;

        public virtual void OnRequest()
        {
            FromPool = true;
        }

        public virtual void OnReturn()
        {
        }

        public void ReturnToPool()
        {
            if (FromPool) GameObject.Return();
            else gameObject.SafeDestroy();
        }
    }
}