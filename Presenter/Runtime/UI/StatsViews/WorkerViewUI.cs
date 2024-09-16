using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Inventories;
using Soul.Model.Runtime.Peoples.Workers;
using Soul.Model.Runtime.Utils;
using TMPro;
using UnityEngine;

namespace Soul.Presenter.Runtime.UI.StatsViews
{
    [System.Serializable]
    public class WorkerViewUI : PreviewStats
    {
        [SerializeField] protected TMPFormat workerText;
        private PlayerFarmReference _playerFarmReference;
        
        public void Setup(PlayerFarmReference playerFarmReference)
        {
            _playerFarmReference = playerFarmReference;
            SetupPreview(_playerFarmReference.workerPreview);
            SetupToggle();
            SetWorker();
            _playerFarmReference.workerInventory.OnItemChanged += InventoryOnItemChanged;
        }

        private void InventoryOnItemChanged(InventoryChangeEventArgs<WorkerType, int> obj)
        {
            SetWorker();
        }

        private void SetWorker()
        {
            workerText.SetTextFloat(GetTotatWorkerCount());
        }

        float GetTotatWorkerCount()
        {
            float totalWorkerCount = 0;
            foreach (var (workerType, value) in _playerFarmReference.workerInventory.GetAll())
            {
                totalWorkerCount += value;
            }
            return totalWorkerCount;
        }

        public override void Dispose()
        {
            base.Dispose();
            _playerFarmReference.workerInventory.OnItemChanged -= InventoryOnItemChanged;
        }

        public override GameObject LoadComponents(GameObject gameObject, string title)
        {
            var container= base.LoadComponents(gameObject, title);
            workerText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("current");
            return container;
        }
    }
}