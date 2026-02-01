using System.Collections;
using InGame.GameConfiguration;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace InGame.Characters.Player.Behaviors
{
  [HideMonoScript]
  public class PlayerDamages : MonoBehaviour, IPlayerBehavior, IPushable
  {
    [Title("Components")]
    [SerializeField] private CharacterController characterController;

    private StateMachine<PlayerState> _state;
    private PlayerConfiguration _configuration;
    private Coroutine _damageCoroutine;
    private float _invincibilityDuration;

    public void Initialize(StateMachine<PlayerState> state) {
      _state = state;
      _configuration = Configuration.Instance.Player;
    }

    public void UpdateBehavior() {
      UpdateInvincibleStatus();
    }

    public void Push(Vector3 direction) {
      if (_state.Has(PlayerState.IsInvincible)) return;

      if (_damageCoroutine != null) {
        StopCoroutine(_damageCoroutine);
      }

      _state.Add(PlayerState.IsInvincible);
      _invincibilityDuration = 0f;
      _damageCoroutine = StartCoroutine(PushCoroutine(direction));
    }

    private void UpdateInvincibleStatus() {
      if (!_state.Has(PlayerState.IsInvincible)) return;

      _invincibilityDuration += Time.deltaTime;
      if (_invincibilityDuration > Configuration.Instance.Player.DamageInvincibilityTimeFrame) {
        _state.Remove(PlayerState.IsInvincible);
      }
    }

    private IEnumerator PushCoroutine(Vector3 direction) {
      var easingType = _configuration.DamageEasingType;
      var duration = _configuration.DamageDuration;
      var distance = _configuration.DamageDistance;

      var elapsedTime = 0f;
      var elapsedDistance = 0f;

      while (elapsedTime < _configuration.DamageDuration) {
        elapsedTime += Time.deltaTime;

        var frameTotalDistance = Easing.FromType(easingType, elapsedTime / duration) * distance;
        var frameDistance = frameTotalDistance - elapsedDistance;
        elapsedDistance += frameDistance;

        characterController.Move(direction * frameDistance);
        yield return null;
      }
    }
  }
}
