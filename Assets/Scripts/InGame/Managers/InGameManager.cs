using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Managers
{
  [HideMonoScript]
  public class InGameManager : MonoBehaviour
  {
    [SerializeField]
    private MonoBehaviour[] updatableManagers;

    private void Update() {
      foreach (var manager in updatableManagers) {
        if (manager is IUpdatableManager updatableManager) {
          updatableManager.OnUpdate();
        }
      }
    }
  }
}
