using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner main;
    public Winner WIN;

    public static Difficulty selectedDifficulty = Difficulty.Normal;
    public enum Difficulty { Easy, Normal, Hard }

    [System.Serializable]
    public class DifficultyProfile
    {
        public string name;
        public float scalingFactor;
        public int finalWave;
        public GameObject bossPrefab;
    }

    [Header("Difficulty Settings")]
    [SerializeField] private DifficultyProfile easySettings;
    [SerializeField] private DifficultyProfile normalSettings;
    [SerializeField] private DifficultyProfile hardSettings;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] TextMeshProUGUI currentWaveUI;

    [Header("Base Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    public int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps;
    private bool isSpawning = false;
    private GameObject activeBoss;
    private bool bossSpawned = false;

    private DifficultyProfile currentProfile;

    private void Awake()
    {
        main = this;
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (bossSpawned && activeBoss == null)
        {
            //Debug.Log("Boss detekován jako mrtvý (je null). Spouštím WIN.");
            bossSpawned = false;
            WIN.Setup();
            return;
        }

        if (bossSpawned && activeBoss != null) return;

        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    public void SetupDifficulty()
    {
        switch (selectedDifficulty)
        {
            case Difficulty.Easy:
                currentProfile = easySettings;
                break;
            case Difficulty.Normal:
                currentProfile = normalSettings;
                break;
            case Difficulty.Hard:
                currentProfile = hardSettings;
                break;
            default:
                currentProfile = normalSettings;
                break;
        }

        UpdateWaveUI();
        //Debug.Log($"Obtížnost nastavena na: {selectedDifficulty}, Max vln: {currentProfile.finalWave}");
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
        //Debug.Log("Nepřítel zničen! Zbývá živých: " + enemiesAlive);
    }

    private IEnumerator StartWave()
    {
        if (currentProfile == null) SetupDifficulty();

        UpdateWaveUI();

        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    public void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;

        if (bossSpawned)
        {
            //Debug.Log("Boss poražen! Spouštím WIN.");
            WIN.Setup();
            return; 
        }

        if (currentWave >= currentProfile.finalWave && !bossSpawned)
        {
            //Debug.Log("Finální vlna dokončena. Přichází Boss.");
            Boss();
            return;
        }

        currentWave++;
        StartCoroutine(StartWave());
    }

    private void Boss()
    {
        GameObject prefabToSpawn = currentProfile.bossPrefab;

        activeBoss = Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);

        bossSpawned = true;

    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[index], LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, currentProfile.scalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, currentProfile.scalingFactor), 0f, enemiesPerSecondCap);
    }

    private void UpdateWaveUI()
    {
        if (currentProfile != null)
        {
            currentWaveUI.text = "Wave " + currentWave.ToString() + "/" + currentProfile.finalWave.ToString();
        }
    }
}