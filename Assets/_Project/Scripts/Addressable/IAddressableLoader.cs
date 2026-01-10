using Cysharp.Threading.Tasks;

namespace Asteroids.Scripts.Addressable
{
    public interface IAddressableLoader
    {
        public UniTask<T> Load<T>(AddressableId addressableId);
        public void Unload(AddressableId addressableId);
    }
}