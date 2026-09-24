using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [System.Serializable]
    public class EnemySpawnData
    {
        [Header("Enemy (Hierarchy la irukura enemy)")]
        public Enemy enemy;

        [Header("Patrol")]
        public Transform pointA;
        public Transform pointB;

        [HideInInspector] public Vector3 spawnPosition;
        [HideInInspector] public Quaternion spawnRotation;
    }

    [SerializeField] private EnemySpawnData[] enemies;

    private Transform player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Original position save
        foreach (EnemySpawnData data in enemies)
        {
            if (data.enemy != null)
            {
                data.spawnPosition = data.enemy.transform.position;
                data.spawnRotation = data.enemy.transform.rotation;
            }
        }
    }

    private void Start()
    {
        PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (playerMovement != null)
            player = playerMovement.transform;

        foreach (EnemySpawnData data in enemies)
        {
            if (data.enemy != null)
            {
                data.enemy.SetPlayer(player);
                data.enemy.SetPatrolPoints(data.pointA, data.pointB);
            }
        }
    }

    // Enemy death animation mudinja apram Enemy inga call pannum
    public void OnEnemyDied(Enemy deadEnemy)
    {
        foreach (EnemySpawnData data in enemies)
        {
            if (data.enemy != deadEnemy)
                continue;

            data.enemy.gameObject.SetActive(false);

            if (data.pointA != null) data.pointA.gameObject.SetActive(false);
            if (data.pointB != null) data.pointB.gameObject.SetActive(false);

            return;
        }
    }

    // Player death / respawn la call aagum
    public void ResetAllEnemies()
    {
        foreach (EnemySpawnData data in enemies)
        {
            if (data.enemy == null)
                continue;

            // Patrol points active
            if (data.pointA != null) data.pointA.gameObject.SetActive(true);
            if (data.pointB != null) data.pointB.gameObject.SetActive(true);

            // Enemy active
            data.enemy.gameObject.SetActive(true);

            // Player + patrol points marubadi set
            data.enemy.SetPlayer(player);
            data.enemy.SetPatrolPoints(data.pointA, data.pointB);

            // Position, health, state ellam reset
            data.enemy.ResetEnemy(data.spawnPosition, data.spawnRotation);
        }
    }
}