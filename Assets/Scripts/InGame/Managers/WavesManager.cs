using System.Collections.Generic;
using InGame.Characters;
using InGame.GameConfiguration;
using InGame.Managers.Map;
using InGame.Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Managers
{
  [HideMonoScript]
  public class WavesManager : AbstractManager<WavesManager>, IUpdatableManager
  {
    [SerializeField]
    private float spawnFrequency = 1f;

    private readonly List<Enemy> _enemies = new();
    private readonly List<Enemy> _enemiesToRemove = new();

    private float _elapsedTime = 0f;

    public void OnUpdate() {
      TrySpawn();
      UpdateEnemies();
    }

    public void RemoveEnemy(Enemy enemy) {
      _enemiesToRemove.Add(enemy);
      Destroy(enemy.gameObject);
    }

    private void TrySpawn() {
      _elapsedTime += Time.deltaTime;

      if (_elapsedTime < spawnFrequency) return;

      _elapsedTime -= spawnFrequency;

      var enemiesList = Configuration.Instance.Enemies.List;
      var enemyToSpawn = enemiesList[Random.Range(0, enemiesList.Length)];
      var destinations = MapManager.Instance.Destinations;

      var spawnPoint = GetRandomSpawnPosition();
      var destination = destinations[Random.Range(0, destinations.Count)];

      var newEnemy = Instantiate(enemyToSpawn, spawnPoint.WorldPosition, Quaternion.identity);
      newEnemy.SetDestination(destination);

      _enemies.Add(newEnemy);
    }

    private GridPosition GetRandomSpawnPosition() {
      var mapSize = Configuration.Instance.Map.Size;
      var perimeterLength = 2 * (mapSize.Width + mapSize.Depth) - 4;
      var randomPoint = Random.Range(0, perimeterLength);

      if (randomPoint < mapSize.Width) {
        return new GridPosition(randomPoint, 0);
      }

      randomPoint -= mapSize.Width;

      if (randomPoint < mapSize.Depth - 1) {
        return new GridPosition(mapSize.Width - 1, randomPoint + 1);
      }

      randomPoint -= (mapSize.Depth - 1);

      if (randomPoint < mapSize.Width - 1) {
        return new GridPosition(mapSize.Width - 2 - randomPoint, mapSize.Depth - 1);
      }

      randomPoint -= (mapSize.Width - 1);

      return new GridPosition(0, mapSize.Depth - 1 - randomPoint);
    }

    private void UpdateEnemies() {
      foreach (var enemy in _enemies) {
        enemy.OnUpdate();
      }

      foreach (var enemy in _enemiesToRemove) {
        _enemies.Remove(enemy);
      }

      _enemiesToRemove.Clear();
    }
  }
}
