using _Project.Scripts.Multiplayer.Pooling;
using Asteroids.Scripts.Weapons.Projectile;
using Fusion;
using UnityEngine;

public sealed class ProjectilePoolAdapter : IPoolAdapter
{
    private readonly ProjectilePool _pool;

    public ProjectilePoolAdapter(ProjectilePool pool)
    {
        _pool = pool;
    }

    public NetworkObject Spawn(Vector3 position, Quaternion rotation)
    {
        Projectile instance = _pool.Spawn(position, rotation, null, null, null);
        if (instance == null)
            return null;

        return instance.GetComponent<NetworkObject>();
    }

    public void Despawn(NetworkObject instance)
    {
        if (instance == null)
            return;

        Projectile projectile = instance.GetComponent<Projectile>();
        if (projectile != null)
            _pool.Despawn(projectile);
    }
}