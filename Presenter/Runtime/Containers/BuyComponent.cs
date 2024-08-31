using Soul.Controller.Runtime.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Containers
{
    public class BuyComponent : MonoBehaviour
    {
        public Image backgroundImage;
        public Image iconImage;
        public TMPFormat priceTMPFormat;

        public void Setup(Sprite icon, int price, Sprite background)
        {
            iconImage.sprite = icon;
            priceTMPFormat.SetTextFloat(price);
            backgroundImage.sprite = background;
        }
    }
}