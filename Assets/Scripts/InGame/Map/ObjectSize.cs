using UnityEngine;

namespace InGame.Map
{
  public readonly struct ObjectSize
  {
    public int Width { get; }
    public int Depth { get; }
    public Vector3 WorldOffset => new(Mathf.Floor(Width / 2f), 0, Mathf.Floor(Depth / 2f));
    public Vector3 ToVector3 => new Vector3(Width, 1f, Depth);

    public ObjectSize(int width, int depth) {
      Width = width;
      Depth = depth;
    }
  }
}
