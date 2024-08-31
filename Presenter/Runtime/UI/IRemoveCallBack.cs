namespace Soul.Presenter.Runtime.UI
{
    public interface IRemoveCallBack<in T>
    {
        public void RemoveSelf(T self);
    }
}