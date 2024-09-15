using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EnemySpawning : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyPrefab;
    [SerializeField] private GameObject gameManager;
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 5f;
    public int minEnemyCount = 3;
    public int maxEnemyCount = 10;
    public bool canSpawn = true;
    public Transform EventSystem;
    private EnemyType[] enemyTypes = new EnemyType[] { EnemyType.Broccoli, EnemyType.Carrot, EnemyType.Corn, EnemyType.Cauliflower, EnemyType.Eggplant, EnemyType.Tomato };

    float spawnRadiusPlayer = 20f, randomRadius = 10f;
    float spawnRadiusObelisk = 10f;
    Vector3 obeliskPosition;

    void Start()
    {
        obeliskPosition = GameObject.Find("Obelisk").transform.position;
        EventSystem = GameObject.Find("EventSystem").transform;
        // enemyPrefab = Resources.Load<GameObject>("/Resources/Prefabs/EnemyExample");
        StartCoroutine(SpawnEnemies());
    }

    private Vector3 GetRandomSpawn()
    {
        var playerPosition = GameObject.Find("Player").transform.position;
        var random = Random.Range(0, randomRadius);
        var random2 = Random.Range(0, randomRadius);
        Vector3 spawnPosition = new Vector3(playerPosition.x + random + (Random.value < 0.5f ? -spawnRadiusPlayer : spawnRadiusPlayer), playerPosition.y + random2 + (Random.value < 0.5f ? -spawnRadiusPlayer : spawnRadiusPlayer), 0);
        //Debug.Log(spawnPosition);
        if (Vector3.Distance(spawnPosition, obeliskPosition) < spawnRadiusObelisk)
            return GetRandomSpawn();
        else return spawnPosition;
    }

    IEnumerator SpawnEnemies()
    {
        Debug.Log("Spawning enemies...");
        while (canSpawn)
        {
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);

            int enemyCount = (int)Random.Range(minEnemyCount * Mathf.Floor(EventSystem.GetComponent<UIHandlers>().GetScore() * 0.025f), (maxEnemyCount + 1) * Mathf.Ceil(EventSystem.GetComponent<UIHandlers>().GetScore() * 0.025f));
            if (enemyCount <= 0)
            {
                enemyCount = (int)Random.Range(minEnemyCount, maxEnemyCount + 1);
            }
            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 spawnPosition = GetRandomSpawn();
                var clone = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                clone.GetComponent<Enemy>().SetType(enemyTypes[Random.Range(0, enemyTypes.Length)]);
                Debug.Log("spawned");
            }
        }
    }



    void Update()
    {
        // Debug.Log(ScalingFunctions.EnemyScalling(EventSystem.GetComponent<UIHandlers>().GetScore()));
    }
}
