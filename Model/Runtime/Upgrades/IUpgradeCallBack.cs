namespace _Root.Scripts.Model.Runtime.Upgrades
{
    public interface IUpgradeCallBack
    {
        public void OnUpgradeJustStarted(int from, int to);
        public void OnUpgradeResume(int from, int to);
        public void OnUpgradeCancel(int from, int to);
        public void OpUpgradeComplete(int from, int to);
    }
}