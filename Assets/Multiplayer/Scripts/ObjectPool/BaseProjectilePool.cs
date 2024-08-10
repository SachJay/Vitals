using Mirror;
using UnityEngine;

public class BaseProjectilePool : MonoBehaviour
{
    public static BaseProjectilePool Instance { get; private set; }

    public Pool<GameObject> Pool;

    [SerializeField] private GameObject prefab;
    [SerializeField] private int prefabCount;
    [SerializeField, ReadOnly] private int currentCount = 0;

    private void Start()
    {
        InitializePool();
        Instance = this;
        NetworkClient.RegisterPrefab(prefab, SpawnHandler, UnspawnHandler);
    }

    private GameObject SpawnHandler(SpawnMessage msg) => GetProjectile(msg.position, msg.rotation);

    private void UnspawnHandler(GameObject spawnedProjectile) => ReturnProjectile(spawnedProjectile);

    private void OnDestroy()
    {
        NetworkClient.UnregisterPrefab(prefab);
    }

    private void InitializePool()
    {
        Pool = new Pool<GameObject>(CreateProjectile, prefabCount);
    }

    private GameObject CreateProjectile()
    {
        GameObject projectile = Instantiate(prefab, transform);
        projectile.name = $"{prefab.name}_pooled_{currentCount}";
        projectile.SetActive(false);
        currentCount++;
        return projectile;
    }

    public GameObject GetProjectile(Vector3 position, Quaternion rotation)
    {
        GameObject projectile = Pool.Get();
        projectile.transform.SetPositionAndRotation(position, rotation);
        projectile.SetActive(true);
        return projectile;
    }

    public void ReturnProjectile(GameObject spawnedProjectile)
    {
        spawnedProjectile.SetActive(false);
        Pool.Return(spawnedProjectile);
    }
}
