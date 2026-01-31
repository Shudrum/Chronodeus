using InGame.GameConfiguration;
using InGame.Map;
using UnityEngine;

namespace InGame.Managers.Map
{
  public static class ProceduralGenerator
  {
    public static void Generate(GridManager gridManager) {
      var generators = Configuration.Instance.Map.Generators;

      foreach (var generator in generators) {
        switch (generator.Type) {
          case GeneratorType.Perlin:
            ExecutePerlinGenerator(generator, gridManager);
            break;
          case GeneratorType.Range:
            ExecuteRangeGenerator(generator, gridManager);
            break;
        }
      }
    }

    private static void ExecutePerlinGenerator(MapGenerator mapGenerator, GridManager gridManager) {
      var mapSize = Configuration.Instance.Map.Size;

      var step = 1f / Mathf.Max(mapSize.Width, mapSize.Depth);
      var noiseOffset = new Vector2(Random.Range(0f, 10000f), Random.Range(0f, 10000f));

      for (var x = 0; x < mapSize.Width; x++) {
        for (var y = 0; y < mapSize.Depth; y++) {
          var value = Mathf.PerlinNoise(
            (x * step + noiseOffset.x) * mapGenerator.Noise,
            (y * step + noiseOffset.y) * mapGenerator.Noise
          );

          if (value < mapGenerator.Density) continue;

          var position = new GridPosition(x, y);
          if (gridManager.TileIsFree(position)) {
            gridManager.PlaceObject(position, mapGenerator.MapObject);
          }
        }
      }
    }

    private static void ExecuteRangeGenerator(MapGenerator mapGenerator, GridManager gridManager) {
      var mapSize = Configuration.Instance.Map.Size;

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
      while (placedObjects < totalObjects && iterations < 1000) {
        iterations++;
        var randomPosition = new GridPosition(Random.Range(minX, maxX), Random.Range(minY, maxY));

        var canBePlaced = true;
        for (var x = 0; x < objectSize.Width; x++) {
          for (var y = 0; y < objectSize.Depth; y++) {
            if (!gridManager.TileIsFree(new GridPosition(randomPosition.X + x, randomPosition.Y + y))) {
              canBePlaced = false;
            }
          }
        }

        if (canBePlaced) {
          gridManager.PlaceObject(randomPosition, mapGenerator.MapObject);
          placedObjects++;
        }
      }
    }
  }
}
