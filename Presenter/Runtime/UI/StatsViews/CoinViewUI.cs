using System;
using Soul.Controller.Runtime.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.UI.StatsViews
{
    [Serializable]
    public class CoinViewUI : StatsView
    {
        [SerializeField] protected TMPFormat coinText;
        [SerializeField] protected TMPFormat coinMaxFormat;
        [SerializeField] protected Image coinIcon;
        

        public void Setup()
        {
        }
    }
}