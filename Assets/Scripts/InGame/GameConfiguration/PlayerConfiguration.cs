using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace InGame.GameConfiguration
{
  [HideMonoScript]
  [CreateAssetMenu(fileName = "Player", menuName = "Chronodeus/Configuration/Player")]
  public class PlayerConfiguration : ScriptableObject
  {
    [Title("Movement")]
    [SerializeField]
    [Range(0.1f, 30f)]
    private float movementSpeed = 2f;

    [SerializeField]
    [Range(0.1f, 30f)]
    private float rotationSpeed = 20f;

    [SerializeField]
    [Range(1f, 20f)]
    private float jumpForce = 10f;

    [SerializeField]
    [Range(0f, 30f)]
    [LabelText("Air movement acceleration")]
    private float airMovement = 0.5f;

    [SerializeField]
    [Range(1f, 50f)]
    private float gravity = 9.81f;

    [Title("Damage")]
    [SerializeField]
    [LabelText("Easing Type")]
    private EasingType damageEasingType;

    [SerializeField]
    [LabelText("Recoil Distance")]
    [Range(0.1f, 5f)]
    private float damageDistance;

    [SerializeField]
    [LabelText("Recoil Duration")]
    [Range(0.1f, 5f)]
    private float damageDuration;

    [SerializeField]
    [LabelText("Invincibility Frame")]
    [Range(0.1f, 5f)]
    private float damageInvincibilityTimeFrame = 0.3f;

    public float MovementSpeed => movementSpeed;
    public float RotationSpeed => rotationSpeed;
    public float JumpForce => jumpForce;
    public float AirMovement => airMovement;
    public float Gravity => gravity;
    public EasingType DamageEasingType => damageEasingType;
    public float DamageDistance => damageDistance;
    public float DamageDuration => damageDuration;
    public float DamageInvincibilityTimeFrame => damageInvincibilityTimeFrame;
  }
}
