using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.PoolAbles;
using UnityEngine;
using UnityEngine.UI;
using UnityProgressBar;

namespace Soul.Presenter.Runtime.Containers
{
    public class BuyComponent : PoolAbleComponent
    {
        public Image backgroundImage;
        public Image iconImage;
        public TMPFormat priceTMPFormat;
        public ProgressBar progressBar;

        public void Setup(Sprite icon, int price, Sprite background, int current, int max)
        {
            iconImage.sprite = icon;
            priceTMPFormat.SetTextFloat(price);
            backgroundImage.sprite = background;
            progressBar.SetValueWithoutNotify(current / (float)max);
        }
    }
}