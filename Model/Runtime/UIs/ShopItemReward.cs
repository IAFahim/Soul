using TMPro;
using UnityEngine;

namespace Soul.Model.Runtime.UIs
{
    public class ShopItemReward : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textValue;

        public void Setup(string value) { textValue.text = value; }
    }
}