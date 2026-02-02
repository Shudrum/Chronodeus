using UnityEngine;

namespace InGame.Map
{
  public static class MapRaycaster
  {
    private static readonly RaycastHit[] Hits = new RaycastHit[5];
    private static readonly LayerMask BlockingMask = LayerMask.GetMask("MapObject");

    public static bool TileIsFree(GridPosition position) {
      return Raycast(position) == 0;
    }

    public static bool MapObjectZoneIsFree(MapObject mapObject, GridPosition position) {
      for (var x = 0; x < mapObject.Size.Width; x++)
      for (var y = 0; y < mapObject.Size.Width; y++) {
        var gridPosition = new GridPosition(position.X + x, position.Y + y);
        if (!TileIsFree(gridPosition)) return false;
      }

      return true;
    }

    private static int Raycast(GridPosition position) {
      var ray = new Ray(position.WorldPosition + Vector3.up * 10f, Vector3.down);
      return Physics.SphereCastNonAlloc(ray, 0.48f, Hits, 20f, BlockingMask);
    }
  }
}
