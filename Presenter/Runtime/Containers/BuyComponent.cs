using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.PoolAbles;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Containers
{
    public class BuyComponent : PoolAbleComponent
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