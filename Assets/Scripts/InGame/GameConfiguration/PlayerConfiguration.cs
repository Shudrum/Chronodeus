using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace InGame.GameConfiguration
{
  [HideMonoScript]
  [CreateAssetMenu(fileName = "Player", menuName = "Chronodeus/Configuration/Player")]
  public class PlayerConfiguration : ScriptableObject
  {
    [Title("Movement")]
    [Range(0.1f, 30f)]
    [SerializeField] private float movementSpeed = 2f;

    [Range(0.1f, 30f)]
    [SerializeField] private float rotationSpeed = 20f;

    [Range(1f, 20f)]
    [SerializeField] private float jumpForce = 10f;

    [Range(0f, 30f)]
    [LabelText("Air movement acceleration")]
    [SerializeField] private float airMovement = 0.5f;

    [Range(1f, 50f)]
    [SerializeField] private float gravity = 9.81f;

    [Title("Damage")]
    [LabelText("Easing Type")]
    [SerializeField] private EasingType damageEasingType;

    [LabelText("Recoil Distance")]
    [Range(0.1f, 5f)]
    [SerializeField] private float damageDistance;

    [LabelText("Recoil Duration")]
    [Range(0.1f, 5f)]
    [SerializeField] private float damageDuration;

    [LabelText("Invincibility Frame")]
    [Range(0.1f, 5f)]
    [SerializeField] private float damageInvincibilityTimeFrame = 0.3f;

    [Title("Hauling")]
    [Range(0.01f, 1f)]
    [SerializeField] private float haulMoveMultiplier = 0.5f;

    [Range(0.1f, 3f)]
    [SerializeField] private float buildDuration = 0.6f;

    [SerializeField] private EasingType buildEasingType = EasingType.Linear;
    [SerializeField] private Material materialOk;
    [SerializeField] private Material materialNotOk;

    [MinMaxSlider(0.1f, 5f)]
    [SerializeField] private Vector2 dropDistance;

    [Range(0.1f, 3f)]
    [SerializeField] private float dropDuration = 0.6f;

    [LabelText("Drop Height Multiplier")]
    [Range(0.1f, 3f)]
    [SerializeField] private float dropHeight = 1f;

    [SerializeField] private AnimationCurve dropHeightCurve;
    [SerializeField] private EasingType dropEasingType;

    public float MovementSpeed => movementSpeed;
    public float RotationSpeed => rotationSpeed;
    public float JumpForce => jumpForce;
    public float AirMovement => airMovement;
    public float Gravity => gravity;
    public EasingType DamageEasingType => damageEasingType;
    public float DamageDistance => damageDistance;
    public float DamageDuration => damageDuration;
    public float DamageInvincibilityTimeFrame => damageInvincibilityTimeFrame;
    public float HaulMoveMultiplier => haulMoveMultiplier;
    public float BuildDuration => buildDuration;
    public EasingType BuildEasingType => buildEasingType;
    public Material MaterialOk => materialOk;
    public Material MaterialNotOk => materialNotOk;
    public Vector2 DropDistance => dropDistance;
    public float DropDuration => dropDuration;
    public float DropHeight => dropHeight;
    public AnimationCurve DropHeightCurve => dropHeightCurve;
    public EasingType DropEasingType => dropEasingType;
  }
}
