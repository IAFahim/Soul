using Alchemy.Inspector;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Reactives;
using Soul.Model.Runtime.Utils;
using TMPro;
using UnityEngine;

namespace Soul.Presenter.Runtime.UI.StatsViews
{
    [System.Serializable]
    public class PreviewStats : StatsView
    {
        [Title("PreviewStats")] [SerializeField]
        protected CanvasGroup previewCanvasGroup;

        [SerializeField] protected TMPFormat previewText;

        protected Reactive<int> PreviewReference;

        public void SetupPreview(Reactive<int> previewReference)
        {
            PreviewReference = previewReference;
            UpdatePreview(0, 0);
            PreviewReference.OnChange += UpdatePreview;
        }

        protected void UpdatePreview(int old, int current)
        {
            if (Mathf.Approximately(current, 0))
            {
                previewCanvasGroup.alpha = 0;
                return;
            }

            previewCanvasGroup.alpha = 1;
            previewText.SetTextFloat(current);
        }

        public override GameObject LoadComponents(GameObject gameObject, string title)
        {
            var container = base.LoadComponents(gameObject, title);
            previewCanvasGroup = container.GetComponentInChildrenWihtName<CanvasGroup>("preview");
            previewText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("preview");
            return container;
        }

        public override void Dispose()
        {
            base.Dispose();
            PreviewReference.OnChange -= UpdatePreview;
        }
    }
}