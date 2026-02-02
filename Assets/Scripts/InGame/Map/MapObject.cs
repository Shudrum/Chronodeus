using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Map
{
  [HideMonoScript]
  public class MapObject : MonoBehaviour
  {
    [Title("Object size")]
    [Range(1, 5)]
    [SerializeField] private int width = 1;

    [Range(1, 5)]
    [SerializeField] private int depth = 1;

    [Title("Interaction")]
    [SerializeField] private bool isHaulable;

    public ObjectSize Size => new(width, depth);
    public bool IsHaulable => isHaulable;

    public GridPosition Position { get; private set; }

    public void InstantiateAt(GridPosition position) {
      Instantiate(this, position.WorldPosition + Size.WorldOffset, Quaternion.identity);
    }
  }
}
