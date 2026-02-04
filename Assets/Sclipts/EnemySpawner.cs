using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnPhase[] phases;

    public int CurrentPhaseIndex;
    private float _timer;


    public void EnemySpawn()
    {
        if (phases.Length == 0) return;

        EnemySpawnPhase phase = phases[CurrentPhaseIndex];

        _timer += Time.deltaTime;

        if (_timer >= phase.spawnInterval)
        {
            _timer = 0f;

            int spawnIndex = Random.Range(0, phase.spawnPoints.Length);

            int enemyIndex = Random.Range(0, phase.enemyTypes.Length);
            EnemyType randomEnemyType = phase.enemyTypes[enemyIndex];

            EnemyBase enemy =
                EnemyObjectPool.Instance.GetEnemy(randomEnemyType);

            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(phase.spawnPoints[spawnIndex].position);
            }
            else
            {
                enemy.transform.position =
                    phase.spawnPoints[spawnIndex].position;
            }
        }
    }
}

[System.Serializable]
public class EnemySpawnPhase
{
    [Header("スポーンポイント")]
    public Transform[] spawnPoints;

    [Header("このフェーズで出る敵の種類（複数）")]
    public EnemyType[] enemyTypes;

    [Header("スポーン間隔")]
    public float spawnInterval;
}