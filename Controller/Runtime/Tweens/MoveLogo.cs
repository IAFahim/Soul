using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Soul.Controller.Runtime.Tweens
{
    public class MoveLogo: MonoBehaviour
    {
        [SerializeField] private float delay;
        [SerializeField] private float duration;
        [SerializeField] private Vector3 targetPosition;
        [SerializeField] private Ease easeType;

        private void OnEnable()
        {
            LMotion.Create(transform.localPosition, targetPosition, duration)
                .WithDelay(delay)
                .WithEase(easeType)
                .WithOnComplete(Callback)
                .BindToLocalPosition(transform);
            
        }

        private void Callback()
        {
            Destroy(gameObject, 0.1f);
        }
    }
}