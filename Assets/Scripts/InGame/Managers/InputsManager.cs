using UnityEngine.InputSystem;

namespace InGame.Managers
{
  public class InputsManager : AbstractManager<InputsManager>
  {
    public bool GamepadActive { get; private set; }
    public bool KeyboardMouseActive { get; private set; }

    private void Start() {
      InputSystem.onActionChange += OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange change) {
      if (change == InputActionChange.ActionPerformed && obj is InputAction action) {
        var device = action.activeControl.device;
        ResetDevices();
        switch (device) {
          case Gamepad:
            GamepadActive = true;
            break;
          case Keyboard or Mouse:
            KeyboardMouseActive = true;
            break;
        }
      }
    }

    private void ResetDevices() {
      GamepadActive = false;
      KeyboardMouseActive = false;
    }
  }
}
