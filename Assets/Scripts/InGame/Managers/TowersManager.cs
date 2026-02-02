using System.Collections.Generic;
using InGame.Towers;

namespace InGame.Managers
{
  public class TowersManager : AbstractManager<TowersManager>, IUpdatableManager
  {
    private readonly List<Tower> _towers = new();

    public void RegisterTower(Tower tower) {
      if (!_towers.Contains(tower)) {
        _towers.Add(tower);
      }
    }

    public void UnregisterTower(Tower tower) {
      if (_towers.Contains(tower)) {
        _towers.Remove(tower);
      }
    }

    public void OnUpdate() {
      foreach (var tower in _towers) {
        if (tower.IsActive) {
          tower.OnUpdate();
        }
      }
    }
  }
}
