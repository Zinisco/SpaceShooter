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
    private GameObject _pickUpPrefab;

    [SerializeField]
    private GameObject[] powerUps;

    private bool _countdownDone = false;

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
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPickUpRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(2f);
            float randomX = Random.Range(-8f, 8f);
            Vector3 posToSpawn = new Vector3(randomX, 7, 0);

            int randPowerUp = Random.Range(0, 3);
            Instantiate(powerUps[randPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f,7f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
