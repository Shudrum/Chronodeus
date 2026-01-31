using System.Collections.Generic;
using InGame.Map;
using UnityEngine;

namespace InGame.Managers.Map
{
  public class GridManager
  {
    private readonly List<GridPosition> _busyTiles = new();
    private readonly Dictionary<GridPosition, MapObject> _elements = new();

    public void PlaceObject(GridPosition gridPosition, MapObject mapObject) {
      _elements.Add(gridPosition, mapObject);
      for (var x = 0; x < mapObject.Size.Width; x++) {
        for (var y = 0; y < mapObject.Size.Depth; y++) {
          _busyTiles.Add(new GridPosition(gridPosition.X + x, gridPosition.Y + y));
        }
      }
    }

    public void InstantiateObjects() {
      foreach (var (position, mapObject) in _elements) {
        var newObject = Object.Instantiate(
          mapObject,
          position.WorldPosition + mapObject.Size.WorldOffset,
          Quaternion.identity
        );
        newObject.SetPosition(position);
      }
    }

    public bool TileIsFree(GridPosition position) {
      return !_busyTiles.Contains(position);
    }

    public void FreeTile(GridPosition position) {
      _busyTiles.Remove(position);
    }
  }
}
