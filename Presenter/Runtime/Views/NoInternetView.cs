﻿using Cysharp.Threading.Tasks;
using Pancake.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Views
{
    public sealed class NoInternetView : View
    {
        [SerializeField] private Button buttonOk;

        protected override UniTask Initialize()
        {
            buttonOk.onClick.AddListener(OnButtonOkPressed);
            return UniTask.CompletedTask;
        }

        private void OnButtonOkPressed()
        {
            PlaySoundClose();
            PopupHelper.Close(transform);
        }
    }
}