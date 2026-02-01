using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Managers
{
  public class InputsManager : AbstractManager<InputsManager>
  {
    private InputActionMap _playerActionMap;

    private InputAction _attackAction;
    private InputAction _moveAction;
    private InputAction _jumpAction;

    public bool GamepadActive { get; private set; }
    public bool KeyboardMouseActive { get; private set; }

    public Vector2 PlayerDirection => _moveAction.ReadValue<Vector2>();
    public bool PlayerJumpPressed => _jumpAction.triggered;
    public bool PlayerAttackPressed => _attackAction.triggered;

    protected override void Awake() {
      base.Awake();

      _playerActionMap = InputSystem.actions.FindActionMap("Player");

      _moveAction = InputSystem.actions.FindAction("Player/Move");
      _attackAction = InputSystem.actions.FindAction("Player/Attack");
      _jumpAction = InputSystem.actions.FindAction("Player/Jump");
    }

    private void Start() {
      InputSystem.onActionChange += OnActionChange;

      EnabledPlayerActions();
    }

    public void EnabledPlayerActions() {
      _playerActionMap.Enable();
    }

    private void OnActionChange(object obj, InputActionChange change) {
      if (change == InputActionChange.ActionPerformed && obj is InputAction action) {
        var device = action.activeControl.device;
        GamepadActive = device is Gamepad;
        KeyboardMouseActive = device is Keyboard or Mouse;
      }
    }
  }
}
