using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("スポーンさせる場所")]
    [SerializeField] private Transform[] _spawnPoint;
    [Header("スポーン間隔")]
    [SerializeField] private float _spawnTime;

   　private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _spawnTime)
        {
            int a = Random.Range(0, _spawnPoint.Length);
            EnemyBase enemy = EnemyObjectPool.Instance.GetEnemy(EnemyType.Nomal);
            enemy.transform.position = _spawnPoint[a].position;
            _timer = 0;
        }
    }
}
