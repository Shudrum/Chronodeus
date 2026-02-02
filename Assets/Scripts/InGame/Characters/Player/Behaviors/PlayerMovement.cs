using InGame.Extensions;
using InGame.GameConfiguration;
using InGame.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace InGame.Characters.Player.Behaviors
{
  [HideMonoScript]
  public class PlayerMovement : MonoBehaviour, IPlayerBehavior
  {
    [Title("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Animator animator;

    private int AnimatorMovementSpeed { get; } = Animator.StringToHash("MovementSpeed");
    private int AnimatorInAir { get; } = Animator.StringToHash("InAir");

    private StateMachine<PlayerState> _state;
    private PlayerConfiguration _configuration;
    private InputsManager _inputs;
    private float _verticalAcceleration;
    private Vector2 _lookDirection;
    private Vector2 _movementDirection;

    public void Initialize(StateMachine<PlayerState> state) {
      _state = state;
      _configuration = Configuration.Instance.Player;
      _inputs = InputsManager.Instance;
    }

    public void UpdateBehavior() {
      UpdateDirection();
      RotateCharacter();
      Move();
      Jump();
      ApplyVerticalAcceleration();
    }

    private void UpdateDirection() {
      _lookDirection = _inputs.PlayerDirection;

      _movementDirection = _state.Has(PlayerState.Grounded)
        ? _lookDirection
        : Vector2.Lerp(_movementDirection, _lookDirection, Time.deltaTime * _configuration.AirMovement);
    }

    private void RotateCharacter() {
      if (_lookDirection == Vector2.zero) return;
      if (_state.Has(PlayerState.Attacking)) return;

      var targetRotation = Quaternion.LookRotation(_lookDirection.ToVector3());
      var rotationSpeed = _configuration.RotationSpeed;

      visualTransform.rotation = Quaternion.Slerp(
        visualTransform.rotation,
        targetRotation,
        rotationSpeed * Time.deltaTime
      );
    }

    private void Move() {
      var canMove = !_state.Has(PlayerState.Attacking)
                    && !_state.Has(PlayerState.HaulingAnimated)
                    && _movementDirection != Vector2.zero;

      var movementSpeed = _configuration.MovementSpeed;
      if (_state.Has(PlayerState.Hauling)) {
        movementSpeed *= _configuration.HaulMoveMultiplier;
      }

      if (canMove) {
        characterController.Move(_movementDirection.ToVector3() * (movementSpeed * Time.deltaTime));
      }

      animator.SetFloat(AnimatorMovementSpeed, _movementDirection.magnitude * movementSpeed);
    }

    private void Jump() {
      var canJump = _inputs.PlayerJumpPressed
                    && _state.Has(PlayerState.Grounded)
                    && !_state.Has(PlayerState.Attacking)
                    && !_state.Has(PlayerState.Hauling);

      if (canJump) {
        _verticalAcceleration = _configuration.JumpForce;
        _state.Add(PlayerState.Jumping);
        animator.SetBool(AnimatorInAir, true);
      }
    }

    private void ApplyVerticalAcceleration() {
      if (_verticalAcceleration <= 0f && _state.Has(PlayerState.Grounded)) {
        _state.Remove(PlayerState.Jumping);
        animator.SetBool(AnimatorInAir, false);
        _verticalAcceleration = 0f;
      }

      _verticalAcceleration -= _configuration.Gravity * Time.deltaTime;
      characterController.Move(Vector3.up * _verticalAcceleration * Time.deltaTime);
    }
  }
}
