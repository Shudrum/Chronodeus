using InGame.Characters.Player.Behaviors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.Characters
{
  [HideMonoScript]
  public class Weapon : MonoBehaviour
  {
    [SerializeField]
    private PlayerAttack playerAttack;

    private void Awake() {
      gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
      playerAttack.OnWeaponHit(other);
    }
  }
}
