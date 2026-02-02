using InGame.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InGame.UI
{
  [HideMonoScript]
  public class ButtonGrid : MonoBehaviour
  {
    [SerializeField]
    private Button[] buttons;

    private InputsManager _inputs;
    private Button _lastHoverButton;

    private void Start() {
      foreach (var button in buttons) {
        InitializeButton(button);
      }
    }

    private void OnInputModeChanged(InputsManager.InputMode inputMode) {
      if (inputMode == InputsManager.InputMode.Gamepad && _lastHoverButton != null) {
        EventSystem.current.SetSelectedGameObject(_lastHoverButton.gameObject);
      }

      if (inputMode == InputsManager.InputMode.KeyboardMouse) {
        EventSystem.current.SetSelectedGameObject(null);
      }
    }

    private void OnEnable() {
      _inputs = InputsManager.Instance;
      _inputs.OnInputModeChanged += OnInputModeChanged;

      _lastHoverButton = null;

      if (_inputs.CurrentInputMode == InputsManager.InputMode.Gamepad && buttons.Length > 0) {
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
      }
    }

    private void OnDisable() {
      _inputs.OnInputModeChanged -= OnInputModeChanged;
    }

    private void InitializeButton(Button button) {
      var trigger = new EventTrigger.Entry();
      trigger.eventID = EventTriggerType.PointerEnter;
      trigger.callback.AddListener((data) => OnMouseEnterButton(button));

      button.GetComponent<EventTrigger>().triggers.Add(trigger);
    }

    private void OnMouseEnterButton(Button button) {
      if (_inputs.CurrentInputMode == InputsManager.InputMode.Gamepad) {
        return;
      }

      _lastHoverButton = button;
    }
  }
}
