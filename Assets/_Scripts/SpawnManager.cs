using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _commonPickUps, _uncommonPickUps, _rarePickUps;

    [SerializeField]
    [Range(0, 100)]
    private int _commonPickUpPercent, _uncommonPickUpPercent;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPickUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while(_stopSpawning == false)
        {
            yield return new WaitForSeconds(3f);
            float randomX = Random.Range(-8f, 8f);
            Vector3 posToSpawn = new Vector3(randomX, 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(2f,5f));
        }
    }

    IEnumerator SpawnPickUpRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(2f);
            SpawnRandomWeightedPickUp();
            yield return new WaitForSeconds(Random.Range(3f,5f));      
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void SpawnRandomWeightedPickUp()
    {
        float randomX = Random.Range(-8f, 8f);
        Vector3 posToSpawn = new Vector3(randomX, 7, 0);

        float weightedValue = Random.Range(0, 100);
        Debug.Log("Weighted Value: " + weightedValue);

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
}
