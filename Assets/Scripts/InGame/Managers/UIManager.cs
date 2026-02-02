using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Managers
{
  [HideMonoScript]
  public class UIManager : AbstractManager<UIManager>, IUpdatableManager
  {
    [Title("Pages")]
    [SerializeField] private GameObject buildTowerPage;

    private GameObject _currentPage;
    private bool _pageVisible;
    private InputsManager _inputs;

    private void Start() {
      _inputs = InputsManager.Instance;
      buildTowerPage.SetActive(false);
    }

    public void OnUpdate() {
      if (_inputs.OpenBuildTowerPressed) {
        OpenBuildTower();
        return;
      }

      if (_inputs.UICancelPressed) {
        ClosePage();
      }
    }

    public void OpenBuildTower() {
      OpenPage(buildTowerPage);
    }

    public void ClosePage() {
      if (!_pageVisible) return;

      _currentPage.SetActive(false);
      _pageVisible = false;

      _inputs.EnabledPlayerActions();
    }

    private void OpenPage(GameObject page) {
      page.SetActive(true);

      if (_pageVisible) {
        _currentPage.SetActive(false);
      }

      _pageVisible = true;
      _currentPage = page;

      _inputs.DisablePlayerActions();
    }
  }
}
