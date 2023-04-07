using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject _bossPrefab;

    [SerializeField]
    private GameObject[] _commonPickUps, _uncommonPickUps, _rarePickUps;

    [SerializeField]
    private GameObject[] _commonEnemy, _uncommonEnemy, _rareEnemy;

    [SerializeField]
    [Range(0, 100)]
    private int _commonPickUpPercent, _uncommonPickUpPercent;

    [SerializeField]
    [Range(0, 100)]
    private int _commonEnemyPercent, _uncommonEnemyPercent;

    public bool stopSpawning = false;
    private bool _isPlayerAlive;
    private bool _canSpawnBoss = false;
    private bool _isBossAlive = true;

    public int waveNumber;

    private int _enemiesDead;
    private int _maxEnemies;
    private int _enemiesLeftToSpawn;

    private UIManager _uiManager;

    private void Start()
    {
        _isPlayerAlive = true;

        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("UI_Manager is Null");
        }
    }

    private void Update()
    {
        if(_canSpawnBoss)
        {
            Instantiate(_bossPrefab, transform.position + new Vector3(0,9,0), Quaternion.identity);
            _canSpawnBoss = false;
        }
    }

    public void StartSpawning(int waveNumber)
    {
        
        if (waveNumber <= 5 && _isPlayerAlive)
        {
            stopSpawning = false;
            _enemiesDead = 0;
            this.waveNumber = waveNumber;
            _uiManager.UpdateWaveStartDisplay(this.waveNumber);
            _enemiesLeftToSpawn = this.waveNumber + 5;
            _maxEnemies = this.waveNumber + 5;
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPickUpRoutine());
        }
        else
        {
            //boss battle
            _canSpawnBoss = true;
            StartCoroutine(SpawnBossPickUpRoutine());
        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (stopSpawning == false && _enemiesDead <= _maxEnemies && _isPlayerAlive)
        {
            if (waveNumber > 2)
            {
                SpawnRandomWeightedEnemy();
            }
            else
            {
                float randomX = Random.Range(-8f, 8f);
                Vector3 posToSpawnEnemy = new Vector3(randomX, 7, 0);
                GameObject newEnemy = Instantiate(_commonEnemy[Random.Range(0, _commonEnemy.Length)], posToSpawnEnemy, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }

            _enemiesLeftToSpawn--;

            if(_enemiesLeftToSpawn == 0)
            {
                stopSpawning = true;
            }
 
            yield return new WaitForSeconds(3f);
        }

        StartSpawning(waveNumber + 1);
    }

    IEnumerator SpawnPickUpRoutine()
    {
        while (stopSpawning == false)
        {
            yield return new WaitForSeconds(3f);
            SpawnRandomWeightedPickUp();
            yield return new WaitForSeconds(Random.Range(8f,15f));
        }
    }

    IEnumerator SpawnBossPickUpRoutine()
    {
        while (_isPlayerAlive && _isBossAlive)
        {
            yield return new WaitForSeconds(3f);
            SpawnRandomWeightedPickUp();
            yield return new WaitForSeconds(Random.Range(8f, 15f));
        }
    }

    public void EnemyDead()
    {
        _enemiesDead++;
    }

    public void OnPlayerDeath()
    {
        _isPlayerAlive = false;
    }

    public void StartGame()
    {
        waveNumber = 1;
        StartSpawning(waveNumber);
    }

    public void BossDeath()
    {
        _isBossAlive = false;
    }


    public void SpawnRandomWeightedPickUp()
    {
        float randomX = Random.Range(-8f, 8f);
        Vector3 posToSpawn = new Vector3(randomX, 7, 0);

        float weightedValue = Random.Range(0, 100);

        GameObject[] ObjectsToSpawn;

        if(weightedValue <= _commonPickUpPercent)
        {
            ObjectsToSpawn = _commonPickUps;
        }
        else if(weightedValue <= _commonPickUpPercent + _uncommonPickUpPercent && weightedValue > _commonPickUpPercent)
        {
            ObjectsToSpawn = _uncommonPickUps;
        }
        else
        {
            ObjectsToSpawn = _rarePickUps;
        }

        Instantiate(ObjectsToSpawn[Random.Range(0, ObjectsToSpawn.Length)], posToSpawn, Quaternion.identity);
    }

    public void SpawnRandomWeightedEnemy()
    {
        float randomX = Random.Range(-8f, 8f);
        Vector3 posToSpawnEnemy = new Vector3(randomX, 7, 0);

        float weightedValue = Random.Range(0, 100);

        GameObject[] EnemiesToSpawn;

        if (weightedValue <= _commonEnemyPercent)
        {
            EnemiesToSpawn = _commonEnemy;
        }
        else if (weightedValue <= _commonEnemyPercent + _uncommonEnemyPercent && weightedValue > _commonEnemyPercent)
        {
            EnemiesToSpawn = _uncommonEnemy;
        }
        else
        {
            EnemiesToSpawn = _rareEnemy;
        }

        GameObject newWeightedEnemy = Instantiate(EnemiesToSpawn[Random.Range(0, EnemiesToSpawn.Length)], posToSpawnEnemy, Quaternion.identity);
        newWeightedEnemy.transform.parent = _enemyContainer.transform;
    }
}
