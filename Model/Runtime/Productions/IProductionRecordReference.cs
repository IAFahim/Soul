using Soul.Model.Runtime.Progressions;

namespace Soul.Model.Runtime.Productions
{
    public interface IProductionRecordReference<T> where T : class, ITimeBasedReference
    {
        public T ProductionRecord { get; set; }
    }
}