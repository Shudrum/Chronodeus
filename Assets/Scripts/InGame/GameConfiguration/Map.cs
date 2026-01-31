using InGame.Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.GameConfiguration
{
  [HideMonoScript]
  [CreateAssetMenu(fileName = "Map", menuName = "Chronodeus/Configuration/Map")]
  public class Map : ScriptableObject
  {
    [TabGroup("General")]
    [Title("Size")]
    [SerializeField]
    [Range(1, 100)]
    private int width;

    [TabGroup("General")]
    [SerializeField]
    [Range(1, 100)]
    private int depth;

    [TabGroup("Generation")]
    [SerializeField]
    [ListDrawerSettings(ShowFoldout = false, DefaultExpandedState = true)]
    private MapGenerator[] generators;

    public MapSize Size => new(width, depth);

    public MapGenerator[] Generators => generators;
  }
}
