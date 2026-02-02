using InGame.Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Towers
{
  [HideMonoScript]
  public class Buildable : MapObject
  {
    public bool IsHauled { get; private set; }

    private bool _collidersInitialized;
    private Collider[] _colliders;

    public virtual void Haul() {
      IsHauled = true;
      SetColliderEnabled(false);
    }

    public void Drop() {
      IsHauled = true;
      SetColliderEnabled(true);
    }

    private void InitializeColliders() {
      if (_collidersInitialized) return;

      _colliders = GetComponentsInChildren<Collider>();
      _collidersInitialized = true;
    }

    private void SetColliderEnabled(bool value) {
      InitializeColliders();
      foreach (var c in _colliders) {
        c.enabled = value;
      }
    }
  }
}
