using InGame.Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Towers
{
  [HideMonoScript]
  public class Buildable : MapObject
  {
    public bool IsHauled { get; private set; }

    private Collider[] _colliders;

    private void Awake() {
      _colliders = GetComponentsInChildren<Collider>();
    }

    public void Haul() {
      IsHauled = true;
      SetColliderEnabled(false);
    }

    public void Drop() {
      IsHauled = true;
      SetColliderEnabled(true);
    }

    private void SetColliderEnabled(bool value) {
      foreach (var c in _colliders) {
        c.enabled = value;
      }
    }
  }
}
