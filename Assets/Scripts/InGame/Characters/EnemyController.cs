using System.Collections;
using InGame.GameConfiguration;
using Pathfinding;
using Pathfinding.RVO;
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
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AIDestinationSetter destinationSetter;

    [SerializeField]
    private RichAI richAI;

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private RVOController rvoController;

    private Coroutine _damageCoroutine;

    // TODO: Magic number to configure
    public bool ReachedDestination => richAI.remainingDistance < 0.2f;

    public void UpdateAnimation() {
      animator.SetFloat(AnimatorMovementSpeed, richAI.velocity.magnitude);
    }

    public void SetMovementSpeed(float movementSpeed) {
      richAI.acceleration = movementSpeed * AccelerationRatio;
      richAI.maxSpeed = movementSpeed;
    }

    public void SetDestination(Transform destination) {
      destinationSetter.target = destination;
      rvoController.priority = Random.Range(0.01f, 0.99f);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
      if (hit.collider.CompareTag("Player")) {
        hit.gameObject.GetComponent<PlayerController>().Push(hit.normal * -1f);
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
