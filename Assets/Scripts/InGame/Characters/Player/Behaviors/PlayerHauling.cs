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

        var offsetX = Mathf.RoundToInt(forward2D.x);
        var offsetZ = Mathf.RoundToInt(forward2D.y);

        _targetPosition = new GridPosition(
          currentPosition.X + offsetX,
          currentPosition.Y + offsetZ
        );

        var distance = Vector3.Distance(
          visualTransform.position,
          _targetPosition.WorldPosition
        );

        // TODO: Magic number to configure
        const float maxDistance = 1.2f;

        if (distance <= maxDistance) {
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

        _hauledCursor.position = _targetPosition.WorldPosition;
      }
    }

    private void DropHauledBuildable() {
      var canBuild = _state.Has(PlayerState.Hauling)
                     && !_state.Has(PlayerState.HaulingAnimated)
                     && MapRaycaster.TileIsFree(_targetPosition)
                     && _inputs.PlayerAttackPressed
                     && _hauledCursor.gameObject.activeSelf;

      if (canBuild) {
        _hauledBuildable.transform.position = _targetPosition.WorldPosition;
        _hauledBuildable.transform.rotation = Quaternion.identity;
        _hauledBuildable.transform.parent = null;
        _hauledBuildable.Drop();
        _hauledBuildable = null;
        StopBuild();
      }
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
