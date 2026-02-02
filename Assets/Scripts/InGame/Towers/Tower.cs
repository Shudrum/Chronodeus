using InGame.Characters;
using InGame.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Towers
{
  public class Tower : Buildable
  {
    [Title("Mesh definition")]
    [SerializeField] private Transform yRotationTransform;
    [SerializeField] private Transform xzRotationTransform;

    [Title("Tower configuration")]
    [SerializeField] private float range;

    public bool IsActive { get; private set; }
    public bool HaveTarget { get; private set; }
    public Enemy Target { get; private set; }
    public float SquaredRange { get; private set; }
    public bool HaveYRotationTransform { get; private set; }
    public bool HaveXZRotationTransform { get; private set; }

    private Transform _transform;
    private WavesManager _wavesManager;

    private void Awake() {
      SquaredRange = range * range;
      HaveYRotationTransform = yRotationTransform != null;
      HaveXZRotationTransform = xzRotationTransform != null;

      _transform = transform;
      _wavesManager = WavesManager.Instance;
    }

    public override void Haul() {
      base.Haul();
      IsActive = false;
    }

    public void Enable() {
      IsActive = true;
    }

    public void OnUpdate() {
      EnsureTargetInRange();
      SearchForTarget();
      LookAtTarget();
    }

    private void EnsureTargetInRange() {
      if (!HaveTarget) return;
      if (!CheckEnemyInRange(Target) || Target.IsDestroyed) {
        ReleaseTarget();
      }
    }

    private void SearchForTarget() {
      if (HaveTarget) return;

      Enemy closestEnemy = null;
      var closestDistance = float.MaxValue;

      foreach (var enemy in _wavesManager.Enemies) {
        if (enemy.IsDestroyed) continue;
        if (!CheckEnemyInRange(enemy)) continue;
        if (enemy.RemainingDistance >= closestDistance) continue;

        closestEnemy = enemy;
        closestDistance = enemy.RemainingDistance;
      }

      if (closestEnemy != null) {
        SetTarget(closestEnemy);
      }
    }

    private void LookAtTarget() {
      if (!HaveTarget) return;

      var enemyPosition = Target.Position;

      if (HaveYRotationTransform) {
        var lookAtPoint = new Vector3(enemyPosition.x, yRotationTransform.position.y, enemyPosition.z);
        yRotationTransform.LookAt(lookAtPoint);
      }

      if (HaveXZRotationTransform) {
        xzRotationTransform.LookAt(enemyPosition);
      }
    }

    private void SetTarget(Enemy enemy) {
      Target = enemy;
      HaveTarget = true;
    }

    private void ReleaseTarget() {
      HaveTarget = false;
      Target = null;
    }

    private bool CheckEnemyInRange(Enemy enemy) {
      var diff = enemy.Position - _transform.position;
      var sqrDistance = diff.sqrMagnitude;

      return sqrDistance < SquaredRange;
    }
  }
}
