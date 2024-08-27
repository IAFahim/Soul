using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Model.Runtime.ParticleEffects
{
    [Serializable]
    public struct AddressableParticleEffect
    {
        public bool active;
        public ParticleSystem particleSystem;
        [SerializeField] private AssetReferenceGameObject particleEffectAssetReference;

        public async UniTaskVoid Load(bool play, Transform transform)
        {
            if (!active)
            {
                if (particleSystem == null)
                {
                    var gameObject = await particleEffectAssetReference.InstantiateAsync(transform);
                    particleSystem = gameObject.GetComponent<ParticleSystem>();
                    active = particleSystem;
                    if (play) Play();
                }
            }
            else
            {
                if (play) Play();
            }
        }

        public bool TryGetParticleSystem(out ParticleSystem particle)
        {
            particle = particleSystem;
            return active;
        }

        public void Play()
        {
            if (active) particleSystem.Play();
        }

        public void Stop()
        {
            if (active) particleSystem.Stop();
        }
    }
}