using InGame.Managers.Map;
using UnityEngine;

namespace InGame.Map.Behaviors
{
  [RequireComponent(typeof(MapObject))]
  public class Grass : MonoBehaviour, IHittable
  {
    [SerializeField]
    private GameObject longGrass;

    [SerializeField]
    private GameObject cutGrass;

    private bool _isCut;

    public void OnHit() {
      if (_isCut) return;

      longGrass.SetActive(false);
      cutGrass.SetActive(true);

      MapManager.Instance.GridManager.FreeTile(GetComponent<MapObject>().Position);
    }
  }
}
