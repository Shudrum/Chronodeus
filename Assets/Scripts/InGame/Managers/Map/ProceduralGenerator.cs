using System.Collections;
using InGame.GameConfiguration;
using InGame.Map;
using UnityEngine;

namespace InGame.Managers.Map
{
  public class ProceduralGenerator
  {
    private const int MaxInstantiatePerFrame = 5;

    private readonly MapConfiguration _mapConfiguration = Configuration.Instance.Map;

    public IEnumerator Generate() {
      var generators = _mapConfiguration.Generators;

      foreach (var generator in generators) {
        switch (generator.Type) {
          case GeneratorType.Perlin:
            yield return ExecutePerlinGenerator(generator);
            break;
          case GeneratorType.Range:
            yield return ExecuteRangeGenerator(generator);
            break;
        }
      }
    }

    private IEnumerator ExecutePerlinGenerator(MapGeneratorConfiguration mapGenerator) {
      var mapSize = _mapConfiguration.Size;

      var step = 1f / Mathf.Max(mapSize.Width, mapSize.Depth);
      var noiseOffset = new Vector2(Random.Range(0f, 10000f), Random.Range(0f, 10000f));
      var instantiated = 0;

      for (var x = 0; x < mapSize.Width; x++) {
        for (var y = 0; y < mapSize.Depth; y++) {
          var value = Mathf.PerlinNoise(
            (x * step + noiseOffset.x) * mapGenerator.Noise,
            (y * step + noiseOffset.y) * mapGenerator.Noise
          );

          if (value < mapGenerator.Density) continue;

          var position = new GridPosition(x, y);
          if (MapRaycaster.TileIsFree(position)) {
            mapGenerator.MapObject.InstantiateAt(position);
            instantiated++;
          }

          if (instantiated >= MaxInstantiatePerFrame) {
            instantiated = 0;
            yield return null;
          }
        }
      }

      yield return null;
    }

    private IEnumerator ExecuteRangeGenerator(MapGeneratorConfiguration mapGenerator) {
      var mapSize = _mapConfiguration.Size;

      var totalTiles = mapSize.Width * mapSize.Depth;
      var totalObjects = mapGenerator.AmountFixed > 0
        ? mapGenerator.AmountFixed
        : Mathf.RoundToInt(totalTiles * mapGenerator.AmountPercent);
      var objectSize = mapGenerator.MapObject.Size;

      var minX = mapGenerator.Padding;
      var minY = mapGenerator.Padding;
      var maxX = mapSize.Width - mapGenerator.Padding - objectSize.Width;
      var maxY = mapSize.Depth - mapGenerator.Padding - objectSize.Depth;

      var iterations = 0;
      var placedObjects = 0;
      var instantiated = 0;
      while (placedObjects < totalObjects && iterations < 1000) {
        iterations++;
        var randomPosition = new GridPosition(Random.Range(minX, maxX), Random.Range(minY, maxY));

        if (MapRaycaster.MapObjectZoneIsFree(mapGenerator.MapObject, randomPosition)) {
          mapGenerator.MapObject.InstantiateAt(randomPosition);
          placedObjects++;
          instantiated++;
        }

        if (instantiated >= MaxInstantiatePerFrame) {
          instantiated = 0;
          yield return null;
        }
      }

      yield return null;
    }
  }
}
