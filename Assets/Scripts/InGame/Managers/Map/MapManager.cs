using System;
using InGame.GameConfiguration;
using InGame.Map;
using UnityEngine;

namespace InGame.Managers.Map
{
  public class MapManager : AbstractManager<MapManager>
  {
    [SerializeField]
    private Transform ground;

    [SerializeField]
    private AstarPath pathfind;

    public GridManager GridManager { get; private set; }

    protected override void Awake() {
      base.Awake();
      GridManager = new GridManager();
      ProceduralGenerator.Generate(GridManager);
    }

    private void Start() {
      ResizeGround();
      ResizePathfind();
      GridManager.InstantiateObjects();
      ComputePathfind();
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos() {
      if (!Application.isPlaying) return;

      var mapSize = Configuration.Instance.Map.Size;

      for (var x = 0; x < mapSize.Width; x++) {
        for (var y = 0; y < mapSize.Depth; y++) {
          var position = new GridPosition(x, y);
          if (!GridManager.TileIsFree(position)) {
            Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
            Gizmos.DrawCube(position.WorldPosition, new Vector3(1f, 0.2f, 1f));
            Gizmos.color = new Color(1f, 0f, 0f);
            Gizmos.DrawWireCube(position.WorldPosition, new Vector3(1f, 0.2f, 1f));
          }
        }
      }
    }
    #endif

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
  }
}
