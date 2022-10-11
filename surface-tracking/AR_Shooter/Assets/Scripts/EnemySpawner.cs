using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDistance;

    public float startSpawnRate;
    public float minSpawnRate;
    public float timeToMinSpawnRate;
    private float spawnRateMod;

    private float lastSpawnTime;
    private float spawnRate;

    public bool canSpawnEnemies;
    public GameObject stageObject;

    // instance
    public static EnemySpawner instance;
    void Awake () { instance = this; }

    void Start ()
    {
        spawnRate = startSpawnRate;
        spawnRateMod = (minSpawnRate - startSpawnRate) / timeToMinSpawnRate;
    }

    void Update ()
    {
        if(!canSpawnEnemies)
            return;

        // every 'spawnRate' seconds, spawn an enemy
        if(Time.time - lastSpawnTime >= spawnRate)
            SpawnEnemy();

        // increase rate of spawning over time
        if(spawnRate > minSpawnRate)
            spawnRate -= spawnRateMod * Time.deltaTime;
    }

    // spawns a new enemy at a random position
    void SpawnEnemy ()
    {
        lastSpawnTime = Time.time;

        Vector3 spawnCircle = Random.onUnitSphere;
        spawnCircle.y = Mathf.Abs(spawnCircle.y);

        Vector3 spawnPos = ShooterCore.instance.transform.position + (spawnCircle * spawnDistance);

       GameObject obj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
       obj.transform.parent = stageObject.transform;
    }
}