using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace InGame.Characters.Player
{
  [HideMonoScript]
  [RequireComponent(typeof(CharacterController))]
  public class PlayerController : MonoBehaviour
  {
    [Title("Components")]
    [SerializeField] private CharacterController characterController;

    [Title("Behaviors")]
    [SerializeField] private MonoBehaviour[] playerBehaviors;

    private readonly StateMachine<PlayerState> _state = new();

    private void Start() {
      foreach (var behavior in playerBehaviors) {
        if (behavior is IPlayerBehavior playerBehavior) {
          playerBehavior.Initialize(_state);
        }
      }
    }

    private void Update() {
      UpdateGroundedState();

      foreach (var behavior in playerBehaviors) {
        if (behavior is IPlayerBehavior playerBehavior) {
          playerBehavior.UpdateBehavior();
        }
      }
    }

    private void UpdateGroundedState() {
      if (characterController.isGrounded) {
        _state.Add(PlayerState.Grounded);
      } else {
        _state.Remove(PlayerState.Grounded);
      }
    }
  }
}
