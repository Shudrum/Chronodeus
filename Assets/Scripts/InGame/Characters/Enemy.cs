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
    [Range(0.1f, 10f)]
    [SerializeField] private float movementSpeed = 1f;

    private Transform _transform;

    public Vector3 Position => _transform.position;
    public float RemainingDistance => Controller.RemainingDistance;

    public EnemyController Controller { get; private set; }
    public bool IsDestroyed { get; private set; }

    private void Awake() {
      Controller = GetComponent<EnemyController>();
      Controller.SetMovementSpeed(movementSpeed);
      _transform = transform;
    }

    public void OnUpdate() {
      Controller.UpdateAnimation();
      if (Controller.ReachedDestination) {
        WavesManager.Instance.RemoveEnemy(this);
      }
    }

    public void SetDestination(Transform destination) {
      Controller.SetDestination(destination);
    }

    public void Destroy() {
      IsDestroyed = true;
      Destroy(gameObject);
    }
  }
}
