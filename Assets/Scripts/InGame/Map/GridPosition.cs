using System;
using UnityEngine;

namespace InGame.Map
{
  public readonly struct GridPosition : IEquatable<GridPosition>
  {
    public int X { get; }
    public int Y { get; }
    public Vector3 WorldPosition => new(X, 0, Y);

    public GridPosition(int x, int y) {
      X = x;
      Y = y;
    }

    public bool Equals(GridPosition other) {
      return other.X == X && other.Y == Y;
    }
  }
}
