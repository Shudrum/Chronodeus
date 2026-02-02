using System.Collections;
using InGame.Characters.Player.Behaviors;
using InGame.GameConfiguration;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace InGame.Characters
{
  [HideMonoScript]
  public class EnemyController : MonoBehaviour, IPushable
  {
    // TODO: Move it on a proper configuration
    private const float AccelerationRatio = 5f;

    private static readonly int AnimatorMovementSpeed = Animator.StringToHash("MovementSpeed");

    [Title("Enemy definition")]
    [SerializeField] private Animator animator;
    [SerializeField] private AIDestinationSetter destinationSetter;
    [SerializeField] private AIPath aiPath;
    [SerializeField] private CharacterController characterController;

    private Coroutine _damageCoroutine;

    // TODO: Magic number to configure
    public bool ReachedDestination => RemainingDistance < 0.3f;
    public float RemainingDistance => aiPath.remainingDistance;

    public void UpdateAnimation() {
      animator.SetFloat(AnimatorMovementSpeed, aiPath.velocity.magnitude);
    }

    public void SetMovementSpeed(float movementSpeed) {
      aiPath.maxAcceleration = movementSpeed * AccelerationRatio;
      aiPath.maxSpeed = movementSpeed;
    }

    public void SetDestination(Transform destination) {
      destinationSetter.target = destination;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
      if (hit.collider.CompareTag("Player")) {
        hit.gameObject.GetComponent<PlayerDamages>().Push(hit.normal * -1f);
      }
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

        characterController.Move(direction * frameDistance);
        yield return null;
      }
    }
  }
}
