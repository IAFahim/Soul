using Pancake.Common;
using Sisus.Init;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Launchers
{
    [Service(typeof(ILoading), FindFromScene = true)]
    public class Loading : MonoBehaviour, ILoadComponent, ILoading
    {
        [SerializeField, Range(0f, 3f)] private float duration;
        [SerializeField] private Slider loadingBar;
        [SerializeField] private TMPFormat txtPercent;

        private float _currentTimeLoading;
        public bool IsLoadingCompleted { get; private set; }

        void ILoadComponent.OnLoadComponents()
        {
            loadingBar = FindFirstObjectByType<Slider>();
            txtPercent.TMP = loadingBar.GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            loadingBar.value = 0;
            IsLoadingCompleted = false;
        }

        private void Update()
        {
            if (!IsLoadingCompleted)
            {
                if (loadingBar.value < 0.4f)
                {
                    loadingBar.value += 1 / duration / 3 * Time.deltaTime;
                    _currentTimeLoading += Time.deltaTime / 3f;
                }
                else
                {
                    loadingBar.value += 1 / duration * Time.deltaTime;
                    _currentTimeLoading += Time.deltaTime;
                }

                txtPercent.SetTextFloat((loadingBar.value * 100).Round());
                if (_currentTimeLoading >= duration) IsLoadingCompleted = true;
            }
        }
    }
}