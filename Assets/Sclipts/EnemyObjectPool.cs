using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum EnemyType
{
    None,
    Nomal,
    Nomal2,
    Nomal3,
    Nomal4,
}

public class EnemyObjectPool : MonoBehaviour
{
    [Serializable]
    public class EnemyData
    {
        public EnemyType EnemyType;
        public EnemyBase EnemyPrefab;
    }

    [SerializeField] private EnemyData[] _enemyPrefabs;

    private Dictionary<EnemyType, ObjectPool<EnemyBase>> _pools = new();

    private static EnemyObjectPool _instance;
    public static EnemyObjectPool Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<EnemyObjectPool>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        foreach(EnemyData data in _enemyPrefabs)
        {
            EnemyType type = data.EnemyType;
            _pools[type] = new ObjectPool<EnemyBase>(
                            createFunc: () => OnCreateObject(type),
                            actionOnGet: (obj) => OnGetObject(obj, type),
                            actionOnRelease: (obj) => OnReleaseObject(obj),
                            actionOnDestroy: (obj) => OnDestroyObject(obj),
                            collectionCheck: true,
                            defaultCapacity: 0,
                            maxSize: 30
                            );
        }
    }

    public EnemyBase GetEnemy(EnemyType type)
    {
        return _pools[type].Get();
    }

    public void ClearAllEnemys()
    {
        foreach(var pool in _pools.Values)
        {
            pool.Clear();
        }
    }

    private EnemyBase OnCreateObject(EnemyType type)
    {
        var prehab = Array.Find(_enemyPrefabs, x => x.EnemyType == type).EnemyPrefab;
        return Instantiate(prehab, transform);
    }

    private void OnGetObject(EnemyBase enemy, EnemyType type)
    {
        enemy.Initialized(() => _pools[type].Release(enemy));

        enemy.gameObject.SetActive(true);
    }

    private void OnReleaseObject(EnemyBase enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyObject(EnemyBase enemy)
    {
        Destroy(enemy.gameObject);
    }
}
