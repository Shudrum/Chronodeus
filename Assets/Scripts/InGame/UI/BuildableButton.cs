using InGame.Characters.Player;
using InGame.Managers;
using InGame.Towers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.UI
{
  [HideMonoScript]
  public class BuildableButton : MonoBehaviour
  {
    [SerializeField] private Buildable buildable;

    public void Build() {
      if (buildable != null) {
        PlayerController.Instance.Hauling.CraftBuildable(buildable);
      }

      UIManager.Instance.ClosePage();
    }
  }
}
