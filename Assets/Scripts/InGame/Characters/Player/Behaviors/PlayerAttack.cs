using System.Collections.Generic;
using InGame.Managers;
using InGame.Map;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace InGame.Characters.Player.Behaviors
{
  [HideMonoScript]
  public class PlayerAttack : MonoBehaviour, IPlayerBehavior
  {
    [Title("Components")]
    [SerializeField] private Animator animator;

    private int AnimatorAttack { get; } = Animator.StringToHash("Attack");

    private readonly List<int> _attackCollidedInstanceIds = new();

    private InputsManager _inputs;
    private StateMachine<PlayerState> _state;
    private bool _attackAnimationStarted;
    private Transform _transform;

    public void Initialize(StateMachine<PlayerState> state) {
      _state = state;
      _transform = transform;
      _inputs = InputsManager.Instance;
    }

    public void UpdateBehavior() {
      StartAttack();
      CheckEndOfAttack();
    }

    public void OnWeaponHit(Collider hitCollider) {
      var colliderInstanceId = hitCollider.GetInstanceID();
      if (_attackCollidedInstanceIds.Contains(colliderInstanceId)) return;

      _attackCollidedInstanceIds.Add(colliderInstanceId);

      if (hitCollider.CompareTag("Enemy")) {
        hitCollider.GetComponent<EnemyController>()
                   .Push(hitCollider.transform.position - _transform.position);
      }

      if (hitCollider.CompareTag("Hittable")) {
        hitCollider.GetComponent<IHittable>().OnHit();
      }
    }

    private void StartAttack() {
      var canAttack = _state.Has(PlayerState.Grounded)
                      && !_state.Has(PlayerState.Attacking)
                      && !_state.Has(PlayerState.Hauling)
                      && _inputs.PlayerAttackPressed;

      if (canAttack) {
        animator.SetBool(AnimatorAttack, true);
        _state.Add(PlayerState.Attacking);
        _attackAnimationStarted = false;
        _attackCollidedInstanceIds.Clear();
      }
    }

    private void CheckEndOfAttack() {
      if (!_state.Has(PlayerState.Attacking)) return;

      var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

      if (!_attackAnimationStarted) {
        _attackAnimationStarted = stateInfo.IsName("Attack");
        return;
      }

      if (!stateInfo.IsName("Attack")) {
        _state.Remove(PlayerState.Attacking);
        animator.SetBool(AnimatorAttack, false);
      }
    }
  }
}
