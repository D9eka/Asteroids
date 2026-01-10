using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Asteroids.Scripts.Addressable
{
    public class UnityAddressableLoader : IAddressableLoader, IDisposable
    {
        private AsyncOperationHandle _cachedObject;
        
        private readonly Dictionary<AddressableId, AsyncOperationHandle> _handles = new();
        
        public async UniTask<T> Load<T>(AddressableId addressableId)
        {
            if (TryGetItemFromDictionary(addressableId, out T result))
            {
                return result;
            }
            
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(addressableId.ToString());
            await handle.Task;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load asset {addressableId}! {handle.OperationException}");
                return default;
            }

            _handles.Add(addressableId, handle);
            return handle.Result;
        }

        public void Unload(AddressableId addressableId)
        {
            if (!_handles.TryGetValue(addressableId, out AsyncOperationHandle handle))
            {
                return;
            }

            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
            _handles.Remove(addressableId);
        }

        public void Dispose()
        {
            foreach (var handlesKey in new List<AddressableId>(_handles.Keys))
            {
                Unload(handlesKey);
            }
        }

        private bool TryGetItemFromDictionary<T>(AddressableId addressableId, out T result)
        {
            if (_handles.ContainsKey(addressableId))
            {
                AsyncOperationHandle currentHandle = _handles[addressableId];
                if (currentHandle.IsValid())
                {
                    if (currentHandle.Result is T currentResult)
                    {
                        result = currentResult;
                        return true;
                    }
                    Debug.LogWarning(
                        $"{addressableId} already loaded as {typeof(T).Name}. Current object will be unloaded.");
                    Unload(addressableId);
                }
                else
                {
                    _handles.Remove(addressableId);
                }
            }
            result = default;
            return false;
        }
    }
}