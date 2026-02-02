using InGame.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
  [HideMonoScript]
  [RequireComponent(typeof(Canvas))]
  [RequireComponent(typeof(GraphicRaycaster))]
  public class GraphicRaycasterToggler : MonoBehaviour
  {
    private GraphicRaycaster _graphicRaycaster;

    private void Start() {
      _graphicRaycaster = GetComponent<GraphicRaycaster>();
      InputsManager.Instance.OnInputModeChanged += OnInputModeChanged;
    }

    private void OnInputModeChanged(InputsManager.InputMode inputMode) {
      _graphicRaycaster.enabled = inputMode == InputsManager.InputMode.KeyboardMouse;
    }
  }
}
