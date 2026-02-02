using System.Collections;
using System.Collections.Generic;
using InGame.GameConfiguration;
using InGame.Managers;
using InGame.Managers.Map;
using InGame.Map;
using InGame.Towers;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace InGame.Characters.Player.Behaviors
{
  [HideMonoScript]
  public class PlayerHauling : MonoBehaviour, IPlayerBehavior
  {
    [Title("Components")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform haulPivot;

    private int AnimatorIsHauling { get; } = Animator.StringToHash("IsHauling");

    private readonly List<MeshRenderer> _hauledCursorRenderers = new();

    private InputsManager _inputs;
    private PlayerConfiguration _configuration;
    private StateMachine<PlayerState> _state;
    private Buildable _hauledBuildable;
    private Transform _hauledCursor;
    private GridPosition _targetPosition;

    public void Initialize(StateMachine<PlayerState> state) {
      _state = state;
      _configuration = Configuration.Instance.Player;
      _inputs = InputsManager.Instance;
    }

    public void UpdateBehavior() {
      UpdateCursorPosition();
      DropHauledBuildable();
    }

    public void CraftBuildable(Buildable buildable) {
      animator.SetBool(AnimatorIsHauling, true);
      StartCoroutine(CraftBuildableCoroutine(buildable));
      InstantiateCursor();
    }

    private IEnumerator CraftBuildableCoroutine(Buildable buildable) {
      _state.Add(PlayerState.HaulingAnimated);
      _state.Add(PlayerState.Hauling);

      var duration = _configuration.BuildDuration;
      var easingType = _configuration.BuildEasingType;

      _hauledBuildable = Instantiate(buildable, haulPivot);
      _hauledBuildable.Haul();

      var hauledBuildableTransform = _hauledBuildable.transform;
      hauledBuildableTransform.localPosition = Vector3.zero;
      hauledBuildableTransform.localScale = Vector3.zero;

      var elapsedTime = 0f;
      while (elapsedTime < duration) {
        elapsedTime += Time.deltaTime;
        hauledBuildableTransform.localScale = Easing.FromType(easingType, elapsedTime / duration) * Vector3.one;
        yield return null;
      }

      _state.Remove(PlayerState.HaulingAnimated);
    }

    private void InstantiateCursor() {
      _hauledCursor = Instantiate(_hauledBuildable).transform;
      _hauledCursorRenderers.Clear();

      _hauledCursor.localScale = Vector3.one;

      var components = _hauledCursor.GetComponentsInChildren<Component>();
      foreach (var component in components) {
        if (component is MeshFilter or Transform) continue;

        if (component is MeshRenderer rendererComponent) {
          _hauledCursorRenderers.Add(rendererComponent);
          rendererComponent.material = _configuration.MaterialOk;
          continue;
        }

        Destroy(component);
      }

      _hauledCursor.gameObject.SetActive(false);
    }

    private void UpdateCursorPosition() {
      var canUpdate = _state.Has(PlayerState.Hauling)
                      && !_state.Has(PlayerState.HaulingAnimated);

      if (canUpdate) {
        var currentPosition = new GridPosition(
          Mathf.RoundToInt(visualTransform.position.x),
          Mathf.RoundToInt(visualTransform.position.z)
        );

        var forward2D = new Vector2(visualTransform.forward.x, visualTransform.forward.z).normalized;


        GridPosition? bestPosition = null;
        var bestDistance = float.MaxValue;

        for (var distance = 1; distance <= 3; distance++) {
          var offsetX = Mathf.RoundToInt(forward2D.x * distance);
          var offsetZ = Mathf.RoundToInt(forward2D.y * distance);

          var candidatePosition = new GridPosition(
            currentPosition.X + offsetX,
            currentPosition.Y + offsetZ
          );

          var worldDistance = Vector3.Distance(
            visualTransform.position,
            candidatePosition.WorldPosition
          );

          if (
            worldDistance >= _configuration.DropDistance.x
            && worldDistance <= _configuration.DropDistance.y
            && worldDistance < bestDistance
          ) {
            bestPosition = candidatePosition;
            bestDistance = worldDistance;
          }
        }

        if (bestPosition.HasValue) {
          _targetPosition = bestPosition.Value;
          _hauledCursor.gameObject.SetActive(true);
          _hauledCursor.position = _targetPosition.WorldPosition + Vector3.up * 0.1f;

          foreach (var meshRenderer in _hauledCursorRenderers) {
            meshRenderer.material = MapRaycaster.TileIsFree(_targetPosition)
              ? _configuration.MaterialOk
              : _configuration.MaterialNotOk;
          }
        } else {
          _hauledCursor.gameObject.SetActive(false);
        }
      }
    }

    private void DropHauledBuildable() {
      var canBuild = _state.Has(PlayerState.Hauling)
                     && !_state.Has(PlayerState.HaulingAnimated)
                     && MapRaycaster.TileIsFree(_targetPosition)
                     && _inputs.PlayerAttackPressed
                     && _hauledCursor.gameObject.activeSelf;

      if (canBuild) {
        StartCoroutine(DropHauledBuildableCoroutine());
        _hauledBuildable.Drop();
        _hauledBuildable = null;
        StopBuild();
      }
    }

    private IEnumerator DropHauledBuildableCoroutine() {
      _state.Add(PlayerState.HaulingAnimated);

      var buildable = _hauledBuildable;
      var buildableTransform = buildable.transform;

      buildableTransform.parent = null;
      var startPosition = buildableTransform.position;
      var startRotation = buildableTransform.rotation;
      var targetDestination = _targetPosition.WorldPosition;

      var elapsedTime = 0f;
      while (elapsedTime <= _configuration.DropDuration) {
        elapsedTime += Time.deltaTime;
        var t = elapsedTime / _configuration.DropDuration;
        var easedT = Easing.FromType(_configuration.DropEasingType, t);

        var positionXZ = Vector3.Lerp(
          new Vector3(startPosition.x,     0, startPosition.z),
          new Vector3(targetDestination.x, 0, targetDestination.z),
          easedT
        );

        var heightOffset = _configuration.DropHeightCurve.Evaluate(easedT) * _configuration.DropHeight;
        var baseY = Mathf.Lerp(startPosition.y, targetDestination.y, easedT);
        var positionY = baseY + heightOffset;

        buildableTransform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);
        buildableTransform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t);

        yield return null;

        MapManager.Instance.UpdatePathAroundObject(_targetPosition, buildable);
      }

      _state.Remove(PlayerState.HaulingAnimated);
    }

    private void StopBuild() {
      if (_hauledBuildable != null) {
        Destroy(_hauledBuildable.gameObject);
      }

      Destroy(_hauledCursor.gameObject);

      _state.Remove(PlayerState.Hauling);
      animator.SetBool(AnimatorIsHauling, false);
    }
  }
}
