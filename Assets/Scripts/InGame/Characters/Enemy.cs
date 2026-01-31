using InGame.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Characters
{
  [HideMonoScript]
  [RequireComponent(typeof(EnemyController))]
  public class Enemy : MonoBehaviour
  {
    [Title("Behavior")]
    [SerializeField]
    [Range(0.1f, 10f)]
    private float movementSpeed = 1f;

    private EnemyController _enemyController;

    private void Awake() {
      _enemyController = GetComponent<EnemyController>();
      _enemyController.SetMovementSpeed(movementSpeed);
    }

    public void OnUpdate() {
      _enemyController.UpdateAnimation();
      if (_enemyController.ReachedDestination) {
        WavesManager.Instance.RemoveEnemy(this);
      }
    }

    public void SetDestination(Transform destination) {
      _enemyController.SetDestination(destination);
    }
  }
}
