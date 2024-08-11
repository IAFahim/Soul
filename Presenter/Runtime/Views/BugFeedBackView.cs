using Cysharp.Threading.Tasks;
using Pancake.UI;
using Soul.Model.Runtime.Reports;
using Trello_Report.Runtime;
using UnityEngine;

namespace Soul.Presenter.Runtime.Views
{
    public class BugFeedBackView : View
    {
        public TrelloReportView trelloReportView;
        public ApiKeys trelloApiKeys;
        public CanvasGroup canvasGroup;

        protected override UniTask Initialize()
        {
            canvasGroup = transform.parent.GetComponent<CanvasGroup>();
            trelloReportView.Setup(canvasGroup, trelloApiKeys.ApiKey(), trelloApiKeys.Token(),
                OnCancelButtonPressed);
            
            return UniTask.CompletedTask;
        }

        private void OnCancelButtonPressed()
        {
            PopupHelper.Close(Transform);
        }
    }
}