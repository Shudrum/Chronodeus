using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Managers
{
  [HideMonoScript]
  [DefaultExecutionOrder(-100)]
  public abstract class AbstractManager<T> : MonoBehaviour where T : AbstractManager<T>
  {
    public static T Instance;

    protected virtual void Awake() {
      Instance = (T)this;
    }

    protected virtual void OnDestroy() {
      if (Instance == this) {
        Instance = null;
      }
    }
  }
}
