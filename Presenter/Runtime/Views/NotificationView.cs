using System;
using Cysharp.Threading.Tasks;
using Pancake.Common;
using Pancake.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Views
{
    public sealed class NotificationView : View
    {
        [SerializeField] private TMP_Text localeText;
        [SerializeField] private Button buttonOk;

        private Action _action;

        protected override UniTask Initialize()
        {
            buttonOk.onClick.AddListener(OnButtonOkPressed);
            return UniTask.CompletedTask;
        }

        private void OnButtonOkPressed()
        {
            C.CallActionClean(ref _action);
            PlaySoundClose();
            PopupHelper.Close(transform);
        }

        public void Setup(string message, Action action)
        {
            localeText.text = message;
            _action = action;
        }
    }
}