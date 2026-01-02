using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("スポーンさせる場所")]
    [SerializeField] private Transform[] _spawnPoint;
    [Header("スポーン間隔")]
    [SerializeField] private float _spawnTime;

   　private float _timer;

    public void EnemySpawn()
    {
        _timer += Time.deltaTime;
        if(_timer > _spawnTime)
        {
            int a = Random.Range(0, _spawnPoint.Length);
            EnemyBase enemy = EnemyObjectPool.Instance.GetEnemy(EnemyType.Nomal);

            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

            if (agent != null)
            {
                agent.Warp(_spawnPoint[a].position);
            }
            else
            {
                enemy.transform.position = _spawnPoint[a].position;
            }
            _timer = 0;
        }
    }
}
