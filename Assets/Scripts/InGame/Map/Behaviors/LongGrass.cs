using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Map.Behaviors
{
  [HideMonoScript]
  public class LongGrass : MonoBehaviour, IHittable
  {
    [SerializeField] private GameObject cutGrass;

    public void OnHit() {
      cutGrass.SetActive(true);
      Destroy(gameObject);
    }
  }
}
