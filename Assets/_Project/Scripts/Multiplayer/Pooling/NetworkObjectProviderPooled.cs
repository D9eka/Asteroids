using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Multiplayer.Pooling
{
    public class NetworkObjectProviderPooled : INetworkObjectProvider
    {
        private readonly NetworkObjectPoolRegistry _registry;
        private readonly Dictionary<NetworkObject, IPoolAdapter> _instancePools = new();

        public bool DelayIfSceneManagerIsBusy { get; set; }

        public NetworkObjectProviderPooled(NetworkObjectPoolRegistry registry)
        {
            _registry = registry;
        }

        public NetworkObjectAcquireResult AcquirePrefabInstance(NetworkRunner runner, in NetworkPrefabAcquireContext context, out NetworkObject instance)
        {
            instance = null;

            if (DelayIfSceneManagerIsBusy && runner.SceneManager.IsBusy)
                return NetworkObjectAcquireResult.Retry;

            NetworkObject prefab;
            try
            {
                prefab = runner.Prefabs.Load(context.PrefabId, isSynchronous: context.IsSynchronous);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[NetworkObjectProviderPooled] Failed to load prefab: {ex}");
                return NetworkObjectAcquireResult.Failed;
            }

            if (!prefab)
                return NetworkObjectAcquireResult.Retry;

            if (_registry != null && _registry.TryGet(prefab, out var adapter))
            {
                Debug.Log($"[NetworkObjectProviderPooled] Spawn from pool: {prefab.name}");
                instance = adapter.Spawn(prefab.transform.position, prefab.transform.rotation);
                if (instance != null)
                    _instancePools[instance] = adapter;
            }
            else
            {
                Debug.Log($"[NetworkObjectProviderPooled] Spawn via Instantiate: {prefab.name}");
                instance = UnityEngine.Object.Instantiate(prefab);
            }

            if (!instance)
                return NetworkObjectAcquireResult.Failed;

            if (context.DontDestroyOnLoad)
                runner.MakeDontDestroyOnLoad(instance.gameObject);
            else
                runner.MoveToRunnerScene(instance.gameObject);

            runner.Prefabs.AddInstance(context.PrefabId);
            return NetworkObjectAcquireResult.Success;
        }

        public void ReleaseInstance(NetworkRunner runner, in NetworkObjectReleaseContext context)
        {
            NetworkObject instance = context.Object;
            if (!context.IsBeingDestroyed)
            {
                if (context.TypeId.IsPrefab)
                {
                    ReleasePrefabInstance(instance);
                }
                else if (context.TypeId.IsSceneObject)
                {
                    UnityEngine.Object.Destroy(instance.gameObject);
                }
                else if (context.IsNestedObject)
                {
                    UnityEngine.Object.Destroy(instance.gameObject);
                }
                else
                {
                    throw new NotImplementedException($"Unknown type id {context.TypeId}");
                }
            }

            if (context.TypeId.IsPrefab)
                runner.Prefabs.RemoveInstance(context.TypeId.AsPrefabId);

            if (instance != null)
                _instancePools.Remove(instance);
        }

        public NetworkPrefabId GetPrefabId(NetworkRunner runner, NetworkObjectGuid prefabGuid)
        {
            return runner.Prefabs.GetId(prefabGuid);
        }

        private void ReleasePrefabInstance(NetworkObject instance)
        {
            if (instance == null)
                return;

            if (_instancePools.TryGetValue(instance, out var adapter))
            {
                Debug.Log($"[NetworkObjectProviderPooled] Despawn to pool: {instance.name}");
                adapter.Despawn(instance);
                return;
            }

            Debug.Log($"[NetworkObjectProviderPooled] Despawn via Destroy: {instance.name}");
            UnityEngine.Object.Destroy(instance.gameObject);
        }
    }
}
