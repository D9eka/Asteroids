using System.Threading.Tasks;

namespace Asteroids.Scripts.Addressable
{
    public interface IAddressableLoader
    {
        public Task<T> Load<T>(AddressableId addressableId);
        public void Unload(AddressableId addressableId);
    }
}