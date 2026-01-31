using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Characters
{
  [HideMonoScript]
  public class CameraPivot : MonoBehaviour
  {
    [SerializeField]
    private Transform transformToFollow;

    private Transform _transform;

    private void Awake() {
      _transform = transform;
    }

    private void Update() {
      var followedPosition = transformToFollow.position;
      followedPosition.y = 0;
      _transform.position = followedPosition;
    }
  }
}
