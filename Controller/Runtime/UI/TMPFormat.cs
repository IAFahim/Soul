using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Soul.Controller.Runtime.UI
{
    [Serializable]
    public struct TMPFormat
    {
        [SerializeField] private string format;
        [SerializeField] private TMP_Text tmp;

        public TMP_Text TMP
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
        public void SetTextInt(int value) => TMP.text = string.Format(format, value.ToString(CultureInfo.InvariantCulture));
        public static implicit operator string(TMPFormat tmpFormat) =>
            tmpFormat.format;

        public static implicit operator TMP_Text(TMPFormat tmpFormat) => 
            tmpFormat.TMP;
    }
}