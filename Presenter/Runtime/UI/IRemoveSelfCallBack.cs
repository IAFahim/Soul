namespace Soul.Presenter.Runtime.UI
{
    public interface IRemoveSelfCallBack<in T>
    {
        public void RemoveSelf(T self);
    }
}