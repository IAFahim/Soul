using Cysharp.Threading.Tasks;

namespace Soul.Controller.Runtime.Constructions
{
    public interface IConstruction
    {
        public UniTask StartConstruction();
        public UniTask EndConstruction();
    }
}