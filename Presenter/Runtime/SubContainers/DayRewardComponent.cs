using TMPro;
using UnityEngine;

namespace _Root.Scripts.Presenter.Runtime.SubContainers
{
    public class DayRewardComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textAmount;

        public void Setup(int amount, bool shouldHide)
        {
            if (shouldHide) textAmount.gameObject.SetActive(false);
            else
            {
                textAmount.gameObject.SetActive(true);
                textAmount.text = $"x{amount}";
            }
        }
    }
}
