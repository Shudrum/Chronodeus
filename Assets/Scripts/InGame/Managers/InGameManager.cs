using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Managers
{
  [HideMonoScript]
  public class InGameManager : MonoBehaviour
  {
    [SerializeField]
    private MonoBehaviour[] managers;

    private void Update() {
      foreach (var manager in managers) {
        if (manager is IUpdatableManager updatableManager) {
          updatableManager.OnUpdate();
        }
      }
    }
  }
}
