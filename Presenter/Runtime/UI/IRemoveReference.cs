namespace Soul.Presenter.Runtime.UI
{
    public interface IRemoveReference<in T>
    {
        public void RemoveSelf(T self);
    }
}