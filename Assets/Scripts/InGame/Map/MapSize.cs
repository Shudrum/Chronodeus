using UnityEngine;

namespace InGame.Map
{
  public readonly struct MapSize
  {
    public int Width { get; }
    public int Depth { get; }
    public Vector3 WorldSize => new(Width, 0, Depth);
    public Vector3 WorldCenter => new(Width / 2f - 0.5f, 0, Depth / 2f - 0.5f);

    public MapSize(int width, int depth) {
      Width = width;
      Depth = depth;
    }
  }
}
