using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Map
{
  [HideMonoScript]
  public class MapObject : MonoBehaviour
  {
    [Title("Object size")]
    [SerializeField]
    [Range(1, 5)]
    private int width = 1;

    [SerializeField]
    [Range(1, 5)]
    private int depth = 1;

    public ObjectSize Size => new(width, depth);

    public GridPosition Position { get; private set; }

    public void SetPosition(GridPosition position) {
      Position = position;
    }
  }
}
