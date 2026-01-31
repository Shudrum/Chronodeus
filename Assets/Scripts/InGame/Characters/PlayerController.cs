using System.Collections;
using System.Collections.Generic;
using InGame.Extensions;
using InGame.GameConfiguration;
using InGame.Map;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace InGame.Characters
{
  [RequireComponent(typeof(CharacterController))]
  public class PlayerController : MonoBehaviour, IPushable
  {
    [Title("Components")]
    [SerializeField]
    private new Camera camera;

    [SerializeField]
    private Transform meshTransform;

    [SerializeField]
    private Animator animator;

    [Title("Character parts")]
    [SerializeField]
    private GameObject idleSword;

    [SerializeField]
    private GameObject attackSword;

    private static readonly int AnimatorAttack = Animator.StringToHash("Attack");
    private static readonly int AnimatorMovementSpeed = Animator.StringToHash("MovementSpeed");
    private static readonly int AnimatorInAir = Animator.StringToHash("InAir");

    private readonly List<int> _attackCollidedInstanceIds = new();

    private InputAction _attackAction;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private bool _attackAnimationStarted;
    private CharacterController _characterController;
    private Coroutine _damageCoroutine;
    private float _invincibleDuration;
    private bool _isAttacking;
    private bool _isInvincible;
    private RaycastHit _raycastHit;
    private Transform _transform;
    private float _verticalAcceleration;
    private Vector2 _movementDirection;
    private Vector2 _lookDirection;

    private void Awake() {
      InputSystem.actions.FindActionMap("Player").Enable();
      _moveAction = InputSystem.actions.FindAction("Player/Move");
      _attackAction = InputSystem.actions.FindAction("Player/Attack");
      _jumpAction = InputSystem.actions.FindAction("Player/Jump");
      _characterController = GetComponent<CharacterController>();
      _transform = GetComponent<Transform>();
    }

    private void Update() {
      UpdateInvincibleStatus();
      CheckEndOfAttack();
      TryStartAttack();
      UpdateDirections();
      MoveCharacter();
      RotateCharacter();
      UpdateAnimator();
      UpdateGravity();
      ApplyVerticalAcceleration();
      TryJump();
    }

    public void Push(Vector3 direction) {
      if (_isInvincible) return;

      var distance = Configuration.Instance.Player.DamageDistance;
      var duration = Configuration.Instance.Player.DamageDuration;
      var easingType = Configuration.Instance.Player.DamageEasingType;

      if (_damageCoroutine != null) StopCoroutine(_damageCoroutine);

      _isInvincible = true;
      _invincibleDuration = 0f;
      _damageCoroutine = StartCoroutine(PushCoroutine(direction, distance, duration, easingType));
    }

    public void WeaponHit(Collider hitCollider) {
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

    private void UpdateDirections() {
      _lookDirection = _moveAction.ReadValue<Vector2>();
      if (_characterController.isGrounded) {
        _movementDirection = _lookDirection;
      }
      else {
        _movementDirection = Vector2.Lerp(
          _movementDirection,
          _lookDirection,
          Time.deltaTime * Configuration.Instance.Player.AirMovement
        );
      }
    }

    private void UpdateInvincibleStatus() {
      if (!_isInvincible) return;

      _invincibleDuration += Time.deltaTime;
      if (_invincibleDuration > Configuration.Instance.Player.DamageInvincibilityTimeFrame)
        _isInvincible = false;
    }

    private void TryStartAttack() {
      if (!_characterController.isGrounded) return;
      if (!_attackAction.triggered) return;
      if (_isAttacking) return;

      animator.SetTrigger(AnimatorAttack);
      _isAttacking = true;
      _attackAnimationStarted = false;
      _attackCollidedInstanceIds.Clear();

      idleSword.SetActive(false);
      attackSword.SetActive(true);
    }

    private void CheckEndOfAttack() {
      if (!_isAttacking) return;

      var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

      if (!_attackAnimationStarted) {
        _attackAnimationStarted = stateInfo.IsName("Attack");
        return;
      }

      if (!stateInfo.IsName("Attack")) {
        _isAttacking = false;
        idleSword.SetActive(true);
        attackSword.SetActive(false);
      }
    }

    private void MoveCharacter() {
      if (_movementDirection.sqrMagnitude < 0.0001f) return;
      if (_isAttacking) return;

      var movementSpeed = Configuration.Instance.Player.MovementSpeed;
      var movement = _transform.rotation * _movementDirection.ToVector3();

      _characterController.Move(movement * (movementSpeed * Time.deltaTime));
    }

    private void RotateCharacter() {
      if (_lookDirection.sqrMagnitude < 0.0001f) return;
      if (_isAttacking) return;

      var targetRotation = Quaternion.LookRotation(_lookDirection.ToVector3());
      var rotationSpeed = Configuration.Instance.Player.RotationSpeed;

      meshTransform.rotation = Quaternion.Slerp(
        meshTransform.rotation,
        targetRotation,
        rotationSpeed * Time.deltaTime
      );
    }

    private void UpdateAnimator() {
      var movementSpeed = Configuration.Instance.Player.MovementSpeed;
      animator.SetFloat(AnimatorMovementSpeed, _movementDirection.magnitude * movementSpeed);
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

    private void UpdateGravity() {
      if (_characterController.isGrounded && _verticalAcceleration <= 0f) {
        _verticalAcceleration = 0f;
      }

      _verticalAcceleration -= Configuration.Instance.Player.Gravity * Time.deltaTime;
    }

    private void ApplyVerticalAcceleration() {
      _characterController.Move(Vector3.up * _verticalAcceleration * Time.deltaTime);
      animator.SetBool(AnimatorInAir, !_characterController.isGrounded);
    }

    private void TryJump() {
      if (!_characterController.isGrounded) return;
      if (!_jumpAction.triggered) return;
      if (_isAttacking) return;

      _verticalAcceleration = Configuration.Instance.Player.JumpForce;
    }
  }
}
