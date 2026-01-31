using UnityEngine;

namespace InGame.Characters
{
  public class Weapon : MonoBehaviour
  {
    [SerializeField]
    private PlayerController playerController;

    private void Awake() {
      gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
      playerController.WeaponHit(other);
    }
  }
}
