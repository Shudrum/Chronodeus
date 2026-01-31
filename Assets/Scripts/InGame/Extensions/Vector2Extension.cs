using UnityEngine;

namespace InGame.Extensions
{
  public static class Vector2Extension
  {
    /// <summary>
    /// Returns a <see cref="Vector3"/> vector from a <see cref="Vector2"/>.
    /// </summary>
    /// <param name="vector2"><see cref="Vector2"/> to convert.</param>
    /// <returns>Resulting <see cref="Vector3"/>.</returns>
    public static Vector3 ToVector3(this Vector2 vector2) {
      return new Vector3(vector2.x, 0, vector2.y);
    }
  }
}
