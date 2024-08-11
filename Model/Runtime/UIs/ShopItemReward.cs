using TMPro;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.UIs
{
    public class ShopItemReward : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textValue;

        public void Setup(string value) { textValue.text = value; }
    }
}