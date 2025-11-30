using UnityEngine;
using UnityEngine.Pool;

public class MagicObjectPool : MonoBehaviour
{
    [SerializeField] private MagicBehavior[] _magicPrefabs;

    private ObjectPool<MagicBehavior>[] _magicPools;

    private static MagicObjectPool _instance;
    public static MagicObjectPool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<MagicObjectPool>();
            }

            return _instance;
        }
    }

    private void Start()
    {
        _magicPools = new ObjectPool<MagicBehavior>[_magicPrefabs.Length];
        for(int i = 0; i < _magicPrefabs.Length; i++)
        {
            int index = i;
            _magicPools[i] = new ObjectPool<MagicBehavior>(
            createFunc: () => OnCreateObject(index),
            actionOnGet: (obj) => OnGetObject(obj,index),
            actionOnRelease: (obj) => OnReleaseObject(obj),
            actionOnDestroy: (obj) => OnDestroyObject(obj),
            collectionCheck: true,
            defaultCapacity: 3,
            maxSize: 10
            );
        }
    }

    // プールからオブジェクトを取得する
    public MagicBehavior GetMagic(int magicIndex)
    {
        if (magicIndex < 0 || magicIndex >= _magicPools.Length)
        {
            Debug.LogWarning("指定された魔法インデックスが範囲外です！");
            return null;
        }

        return _magicPools[magicIndex].Get();
    }

    // プールの中身を空にする
    public void ClearAllMagic()
    {
        foreach (var pool in _magicPools)
        {
            pool.Clear();
        }
    }

    // プールに入れるインスタンスを新しく生成する際に行う処理
    private MagicBehavior OnCreateObject(int index)
    {
        return Instantiate(_magicPrefabs[index], transform);
    }

    // プールからインスタンスを取得した際に行う処理
    private void OnGetObject(MagicBehavior magicObject, int magicIndex)
    {
        magicObject.Initialize(() => _magicPools[magicIndex].Release(magicObject));
        magicObject.gameObject.SetActive(true);
    }

    // プールにインスタンスを返却した際に行う処理
    private void OnReleaseObject(MagicBehavior magicObject)
    {
        Debug.Log("Release");  // EnemyObject側で非アクティブにするのでログ出力のみ。ここで非アクティブにするパターンもある。
    }

    // プールから削除される際に行う処理
    private void OnDestroyObject(MagicBehavior magicObject)
    {
        Destroy(magicObject.gameObject);
    }

    public void SetMagicPrefab()
    {

    }
}
