using System.Collections;
using InGame.GameConfiguration;
using Pathfinding;
using UnityEngine;
using Utils;

namespace InGame.Characters
{
  [RequireComponent(typeof(RichAI))]
  public class EnemyController : MonoBehaviour, IPushable
  {
    private static readonly int AnimatorMovementSpeed = Animator.StringToHash("MovementSpeed");

    [SerializeField]
    private Animator animator;

    private CharacterController _characterController;
    private Coroutine           _damageCoroutine;
    private RichAI              _richAI;

    private void Awake() {
      _characterController = GetComponent<CharacterController>();
      _richAI = GetComponent<RichAI>();
    }

    // TODO: Move it on a manager update to avoid Unity's API calls.
    private void Update() {
      animator.SetFloat(AnimatorMovementSpeed, _richAI.velocity.magnitude);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
      if (hit.collider.CompareTag("Player"))
        hit.gameObject.GetComponent<PlayerController>().Push(hit.normal * -1f);
    }

    public void Push(Vector3 direction) {
      var distance = Configuration.Instance.Player.DamageDistance;
      var duration = Configuration.Instance.Player.DamageDuration;
      var easingType = Configuration.Instance.Player.DamageEasingType;

      if (_damageCoroutine != null) StopCoroutine(_damageCoroutine);

      _damageCoroutine = StartCoroutine(PushCoroutine(direction, distance, duration, easingType));
    }

    private IEnumerator PushCoroutine(
      Vector3 direction,
      float distance,
      float duration,
      EasingType easingType
    ) {
      var elapsedTime = 0f;
      var elapsedDistance = 0f;
      while (elapsedTime < duration) {
        elapsedTime += Time.deltaTime;

        var frameTotalDistance = Easing.FromType(easingType, elapsedTime / duration) * distance;
        var frameDistance = frameTotalDistance - elapsedDistance;
        elapsedDistance += frameDistance;

        _characterController.Move(direction * frameDistance);
        yield return null;
      }
    }
  }
}
