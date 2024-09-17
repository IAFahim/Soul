using System.Globalization;
using Alchemy.Inspector;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.PoolAbles;
using UnityEngine;
using UnityProgressBar;

namespace Soul.Presenter.Runtime.Slots
{
    public class UpgradeSlot : PoolAbleComponent
    {
        [SerializeField] protected ProgressBar currentProgressBar;
        [SerializeField] protected ProgressBar nextProgressBar;

        [SerializeField] protected TMPFormat titleText;

        //{0} {1} {2} => 100% > 120%
        [SerializeField] protected TMPFormat upgradeInfoText;

        private void Awake()
        {
            titleText.StoreFormat();
            upgradeInfoText.StoreFormat();
        }

        [Button]
        public void Setup(string title, float current, float next, float max, bool percentage = false)
        {
            titleText.TMP.text = string.Format(titleText, title);
            string currentString = percentage ? $"{current * 100}%" : current.ToString(CultureInfo.InvariantCulture);
            string nextString = percentage ? $"{next * 100}%" : next.ToString(CultureInfo.InvariantCulture);
            upgradeInfoText.TMP.text = string.Format(upgradeInfoText, currentString, '>', nextString);
            SetProgressBar(current, next, max);
        }

        private void SetProgressBar(float current, float next, float max)
        {
            if (Mathf.Approximately(current, 0))
            {
                currentProgressBar.Value = 0.01f;
                nextProgressBar.Value = next / max;
            }
            else if (Mathf.Approximately(current, max))
            {
                currentProgressBar.Value = .99f;
                nextProgressBar.Value = 1;
            }
            else
            {
                currentProgressBar.Value = current / max;
                nextProgressBar.Value = next / max;
            }
        }
    }
}