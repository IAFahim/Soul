using System;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace Soul.Model.Runtime.ParticleEffects
{
    [Serializable]
    public class AddressableParticleEffect
    {
        [FormerlySerializedAs("active")] [ShowInInspector]
        private bool _isLoaded;

        private ParticleSystem particleSystem;
        [SerializeField] private AssetReferenceGameObject particleEffectAssetReference;

        public async UniTaskVoid Load(bool play, Transform transform)
        {
            if (particleSystem)
            {
                if (play) Play();
            }
            else
            {
                var gameObject = await particleEffectAssetReference.InstantiateAsync(transform).ToUniTask();
                particleSystem = gameObject.GetComponent<ParticleSystem>();
                _isLoaded = particleSystem;
                if (play) Play();
            }
        }

        public bool TryGetParticleSystem(out ParticleSystem particle)
        {
            particle = particleSystem;
            return _isLoaded;
        }

        public void Play()
        {
            if (_isLoaded) particleSystem.Play();
        }

        public void Stop()
        {
            if (_isLoaded) particleSystem.Stop();
        }
    }
}