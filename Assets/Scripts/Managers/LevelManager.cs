using System.Collections;
using UnityEngine;

public enum LevelState
{
    Build, Defence
}

public class LevelManager : Singelton<LevelManager>
{
    public LevelState LevelState
    {
        get;
        private set;
    }

    private bool isSpawning = false;
    public bool IsSpawning()
    {
        CheckLevelState();
        return isSpawning = !isSpawning;
    }

    private Vector3 SpawnPoint = new Vector3(-30, 1, 10);
    private Vector3 Goal = new Vector3(30, 1, 10);

    private readonly float spawnDelay = 1f;

    private void Start()
    {
        SetSpawnPointAndGoal();
        Spawn(spawnDelay);
        LevelState.Equals(LevelState.Build);
    }

    private void CheckLevelState()
    {
        LevelState = isSpawning ? LevelState.Build : LevelState.Defence;
    }

    private void Spawn(float spawnDelay)
    {
        StartCoroutine(ISpawn(spawnDelay));
    }

    private void SetSpawnPointAndGoal()
    {
        ObjectPoolManager.Instance.GetObjectFromPool("SpawnPoint", SpawnPoint, Quaternion.Euler(Vector3.zero));
        ObjectPoolManager.Instance.GetObjectFromPool("Goal", Goal, Quaternion.Euler(Vector3.zero));
    }

    private IEnumerator ISpawn(float spawnDelay)
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (isSpawning)
            {
                int randomEnemy = Random.Range(1, 3);

                var randomEnemyName = randomEnemy < 3 ? "LightEnemy" : "HeavyEnemy";

                ObjectPoolManager.Instance.GetObjectFromPool(randomEnemyName, SpawnPoint, Quaternion.Euler(Vector3.zero));
                yield return null;
            }            
        }
    }
}
