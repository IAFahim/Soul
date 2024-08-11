using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.UI
{
    [Serializable]
    public partial struct TextMeshProUGUIFormat
    {
        [SerializeField] private string format;
        [SerializeField] private TextMeshProUGUI tmp;

        public TextMeshProUGUI TMP
        {
            get => tmp;
            set
            {
                tmp = value;
                StoreFormat();
            }
        }

        public void StoreFormat() => format = TMP.text;

        public void SetTextFloat(float value) => TMP.text = string.Format(format, value.ToString(CultureInfo.InvariantCulture));
        public static implicit operator string(TextMeshProUGUIFormat textMeshProUGUIFormat) =>
            textMeshProUGUIFormat.format;

        public static implicit operator TextMeshProUGUI(TextMeshProUGUIFormat textMeshProUGUIFormat) => 
            textMeshProUGUIFormat.TMP;
    }
}