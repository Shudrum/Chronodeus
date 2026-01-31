using UnityEngine;

namespace Utils.Extensions
{
  public static class Vector2IntExtension
  {
    public static Vector3 ToVector3(this Vector2Int vector2Int) {
      return new Vector3(vector2Int.x, 0f, vector2Int.y);
    }
  }
}
