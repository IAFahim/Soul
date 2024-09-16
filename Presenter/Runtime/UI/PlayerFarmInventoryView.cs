using Pancake;
using Pancake.Common;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.LookUpTables;
using Soul.Presenter.Runtime.UI.StatsViews;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Presenter.Runtime.UI
{
    public class PlayerFarmInventoryView : GameComponent, ILoadComponent
    {
        [FormerlySerializedAs("playerInventoryReference")]
        public PlayerFarmReference playerFarmReference;

        public PriceLookUpTable priceLookUpTable;

        [SerializeField] public LevelXpDayViewUI levelXpDayViewUI;
        [SerializeField] public CurrencyViewUI coinViewUI;
        [SerializeField] public CurrencyViewUI gemViewUI;
        [SerializeField] public WorkerViewUI workerViewUI;
        [SerializeField] public WeightViewUI weightViewUI;
        

        public void OnEnable()
        {
            playerFarmReference.Load();
            levelXpDayViewUI.Setup(playerFarmReference.levelXp, playerFarmReference.xpPreview);
            coinViewUI.Setup(playerFarmReference.coins, playerFarmReference.coinPreview);
            gemViewUI.Setup(playerFarmReference.gems, playerFarmReference.gemsPreview);
            workerViewUI.Setup(playerFarmReference);
            weightViewUI.Setup(playerFarmReference, priceLookUpTable);
        }
        
        public void OnDisable()
        {
            levelXpDayViewUI.Dispose();
            coinViewUI.Dispose();
            gemViewUI.Dispose();
            workerViewUI.Dispose();
            weightViewUI.Dispose();
        }

        void ILoadComponent.OnLoadComponents()
        {
            levelXpDayViewUI.LoadComponents(gameObject, "level");
            coinViewUI.LoadComponents(gameObject, "coin");
            gemViewUI.LoadComponents(gameObject, "gem");
            workerViewUI.LoadComponents(gameObject, "worker");
            weightViewUI.LoadComponents(gameObject, "weight");
        }
    }
}