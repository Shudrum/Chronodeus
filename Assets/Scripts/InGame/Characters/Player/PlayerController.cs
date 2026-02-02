using InGame.Characters.Player.Behaviors;
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
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerAttack attack;
    [SerializeField] private PlayerDamages damages;
    [SerializeField] private PlayerHauling hauling;

    public static PlayerController Instance { get; private set; }

    public PlayerHauling Hauling => hauling;

    private readonly StateMachine<PlayerState> _state = new();

    private void Awake() {
      Instance = this;
    }

    private void Start() {
      movement.Initialize(_state);
      attack.Initialize(_state);
      damages.Initialize(_state);
      hauling.Initialize(_state);
    }

    private void Update() {
      UpdateGroundedState();

      movement.UpdateBehavior();
      attack.UpdateBehavior();
      damages.UpdateBehavior();
      hauling.UpdateBehavior();
    }

    private void OnDestroy() {
      if (Instance == this) {
        Instance = null;
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
