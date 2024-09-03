using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Controller.Runtime.UI.Components
{
    public class IconRequiredMaxComponent : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMPFormat countMaxTMPFormat;
        [SerializeField] private UIShiny uIShiny;

        private void Awake()
        {
            countMaxTMPFormat.StoreFormat();
        }

        public void Setup(Sprite sprite, int required, int max)
        {
            image.sprite = sprite;
            countMaxTMPFormat.TMP.text = string.Format(countMaxTMPFormat, required, max);
            uIShiny.Play();
        }
    }
}