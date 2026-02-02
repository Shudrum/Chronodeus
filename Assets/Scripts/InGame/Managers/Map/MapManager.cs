using System.Collections;
using System.Collections.Generic;
using InGame.GameConfiguration;
using InGame.Map;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Managers.Map
{
  public class MapManager : AbstractManager<MapManager>
  {
    [Title("Components")]
    [SerializeField] private Transform ground;
    [SerializeField] private AstarPath pathfind;

    public List<Transform> Destinations { get; private set; } = new();

    private void Start() {
      StartCoroutine(BuildMapCoroutine());
    }

    public IEnumerator BuildMapCoroutine() {
      ResizeGround();
      ResizePathfind();
      yield return null;

      var generator = new ProceduralGenerator();
      yield return generator.Generate();

      ComputePathfind();
      GatherDestinations();
    }

    public void UpdatePathAroundObject(GridPosition position, MapObject mapObject) {
      var bounds = new Bounds(
        position.WorldPosition + mapObject.Size.WorldOffset,
        mapObject.Size.ToVector3 + Vector3.one
      );
      var guo = new GraphUpdateObject(bounds) {
        updatePhysics = true,
        
      };
      pathfind.UpdateGraphs(bounds);
    }

    private void ResizeGround() {
      var mapSize = Configuration.Instance.Map.Size;
      ground.localScale = mapSize.WorldSize + Vector3.up;
      ground.position = mapSize.WorldCenter;
    }

    private void ResizePathfind() {
      var mapSize = Configuration.Instance.Map.Size;
      var recastGraph = AstarPath.active.data.recastGraph;

      recastGraph.forcedBoundsCenter = mapSize.WorldCenter;
      recastGraph.forcedBoundsSize = mapSize.WorldSize + Vector3.up * 10f;
    }

    private void ComputePathfind() {
      AstarPath.active.data.recastGraph.Scan();
    }

    private void GatherDestinations() {
      var destinations = FindObjectsByType<Destination>(
        FindObjectsInactive.Include,
        FindObjectsSortMode.None
      );
      foreach (var destination in destinations) {
        Destinations.Add(destination.transform);
      }
    }
  }
}
