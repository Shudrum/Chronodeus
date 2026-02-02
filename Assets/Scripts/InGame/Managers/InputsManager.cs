using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Managers
{
  public class InputsManager : AbstractManager<InputsManager>
  {
    public enum InputMode
    {
      KeyboardMouse,
      Gamepad,
    }

    private InputActionMap _playerActionMap;

    private InputAction _attackAction;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _openBuildTowerAction;

    private InputAction _uiCancelAction;

    public InputMode CurrentInputMode { get; private set; }

    public Vector2 PlayerDirection => _moveAction.ReadValue<Vector2>();
    public bool PlayerJumpPressed => _jumpAction.triggered;
    public bool PlayerAttackPressed => _attackAction.triggered;
    public bool OpenBuildTowerPressed => _openBuildTowerAction.triggered;

    public bool UICancelPressed => _uiCancelAction.triggered;

    public event Action<InputMode> OnInputModeChanged;

    protected override void Awake() {
      base.Awake();

      _playerActionMap = InputSystem.actions.FindActionMap("Player");

      _moveAction = InputSystem.actions.FindAction("Player/Move");
      _attackAction = InputSystem.actions.FindAction("Player/Attack");
      _jumpAction = InputSystem.actions.FindAction("Player/Jump");
      _jumpAction = InputSystem.actions.FindAction("Player/Jump");
      _openBuildTowerAction = InputSystem.actions.FindAction("Player/OpenBuildTower");

      _uiCancelAction = InputSystem.actions.FindAction("UI/Cancel");
    }

    private void Start() {
      InputSystem.onActionChange += OnActionChange;
      EnabledPlayerActions();
    }

    protected override void OnDestroy() {
      base.OnDestroy();
      InputSystem.onActionChange -= OnActionChange;
    }

    public void EnabledPlayerActions() {
      _playerActionMap.Enable();
    }

    public void DisablePlayerActions() {
      _playerActionMap.Disable();
    }

    private void OnActionChange(object obj, InputActionChange change) {
      if (change == InputActionChange.ActionPerformed && obj is InputAction action) {
        var device = action.activeControl.device;

        var newInputMode = device switch {
          Gamepad => InputMode.Gamepad,
          _ => InputMode.KeyboardMouse,
        };

        if (newInputMode != CurrentInputMode) {
          CurrentInputMode = newInputMode;
          OnInputModeChanged?.Invoke(CurrentInputMode);
        }
      }
    }
  }
}
