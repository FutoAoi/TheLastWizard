using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum EffectType
{
    None,
    Hit,
    thunder
}

public class EffectObjectPool : MonoBehaviour
{
    [Serializable]
    public class EffectData
    {
        public EffectType EffectType;
        public EffectBehavior EffectPrefab;
    }

    [SerializeField] private EffectData[] _effectPrefabs;

    private Dictionary<EffectType, ObjectPool<EffectBehavior>> _pools = new();

    private static EffectObjectPool _instance;
    public static EffectObjectPool Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<EffectObjectPool>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        foreach(EffectData data in _effectPrefabs)
        {
            EffectType type = data.EffectType;

            _pools[type] = new ObjectPool<EffectBehavior>(
                            createFunc: () => OnCreateObject(type),
                            actionOnGet: (obj) => OnGetObject(obj, type),
                            actionOnRelease: (obj) => OnReleaseObject(obj),
                            actionOnDestroy: (obj) => OnDestroyObject(obj),
                            collectionCheck: true,
                            defaultCapacity: 0,
                            maxSize: 20
                            );
        }
    }

    public EffectBehavior GetEffect(EffectType type)
    {
        return _pools[type].Get();
    }

    public void ClearAllEffects()
    {
        foreach(var pool in _pools.Values)
        {
            pool.Clear();
        }
    }

    private EffectBehavior OnCreateObject(EffectType type)
    {
        var prefab = Array.Find(_effectPrefabs, x => x.EffectType == type).EffectPrefab;
        return Instantiate(prefab, transform);
    }

    private void OnGetObject(EffectBehavior effect,  EffectType type)
    {
        effect.Initialized(() => _pools[type].Release(effect));

        effect.gameObject.SetActive(true);
    }

    private void OnReleaseObject(EffectBehavior effect)
    {
        effect.gameObject.SetActive(false);
    }

    private void OnDestroyObject(EffectBehavior effect)
    {
        Destroy(effect.gameObject);
    }
}
