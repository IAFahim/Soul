using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Indicators;
using UnityEngine;

namespace Soul.Controller.Runtime.Indicators
{
    public class IndicatorProgressCapacity : IndicatorProgress 
    {
        [SerializeField] private TMPFormat currentCapacity;
        [SerializeField] private SpriteRenderer icon;

        protected override void Awake()
        {
            currentCapacity.StoreFormat();
            base.Awake();
        }

        public void Setup(float fill, float duration, bool onCompleteReturn, float current, float max, Sprite sprite)
        {
            Change(current, max);
            Change(sprite);
            base.Setup(fill, duration, onCompleteReturn);
        }
        
        public void Change(float current, float max)
        {
            currentCapacity.SetTextFloat(current, max);
        }
        
        public void Change(Sprite sprite)
        {
            icon.sprite = sprite;
        }
    }
}