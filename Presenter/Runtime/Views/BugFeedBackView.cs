using _Root.Scripts.Model.Runtime.Reports;
using Cysharp.Threading.Tasks;
using Pancake.UI;
using Trello_Report.Runtime;
using UnityEngine;

namespace _Root.Scripts.Presenter.Runtime.Views
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