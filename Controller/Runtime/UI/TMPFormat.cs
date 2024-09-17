using System;
using System.Globalization;
using System.Text;
using QuickEye.Utility;
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

        public void SetTimeFormat(UnityTimeSpan timeSpan)
        {
            //{0} 1d 2h 3m 4s
            StringBuilder time = new StringBuilder();
            if (timeSpan.Days > 0)
                time.Append($"{timeSpan.Days}d ");
            if (timeSpan.Hours > 0)
                time.Append($"{timeSpan.Hours}h ");
            if (timeSpan.Minutes > 0)
                time.Append($"{timeSpan.Minutes}m ");
            if (timeSpan.Seconds > 0)
                time.Append($"{timeSpan.Seconds}s");
            TMP.text = string.Format(format, time);
            
        }

        public void StoreFormat() => format = TMP.text;

        public void SetTextFloat(float value) =>
            TMP.text = string.Format(format, value.ToString(CultureInfo.InvariantCulture));

        public void SetTextFloat(float first, float second) => TMP.text = string.Format(format,
            first.ToString(CultureInfo.InvariantCulture), second.ToString(CultureInfo.InvariantCulture));

        public void SetTextInt(int value) =>
            TMP.text = string.Format(format, value.ToString(CultureInfo.InvariantCulture));

        public static implicit operator string(TMPFormat tmpFormat) =>
            tmpFormat.format;

        public static implicit operator TMP_Text(TMPFormat tmpFormat) =>
            tmpFormat.TMP;
    }
}