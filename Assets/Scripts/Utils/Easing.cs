using UnityEngine;

namespace Utils
{
  public enum EasingType
  {
    Linear,
    OutSine,
    OutQuad,
    OutCubic,
    OutQuart,
    OutQuint,
    OutExpo,
    OutCirc,
    OutBack,
    OutElastic,
    OutBounce,
  }

  /// <summary>
  /// List of easing functions.<br />
  /// Source: https://easings.net
  /// </summary>
  public static class Easing
  {
    public static float FromType(EasingType type, float t) {
      return type switch {
        EasingType.OutSine => OutSine(t),
        EasingType.OutQuad => OutQuad(t),
        EasingType.OutCubic => OutCubic(t),
        EasingType.OutQuart => OutQuart(t),
        EasingType.OutQuint => OutQuint(t),
        EasingType.OutExpo => OutExpo(t),
        EasingType.OutCirc => OutCirc(t),
        EasingType.OutBack => OutBack(t),
        EasingType.OutElastic => OutElastic(t),
        EasingType.OutBounce => OutBounce(t),
        _ => Linear(t),
      };
    }

    public static float Linear(float t) => t;

    public static float OutSine(float t) {
      return Mathf.Sin(t * Mathf.PI / 2f);
    }

    public static float OutQuad(float t) {
      return 1f - (1f - t) * (1f - t);
    }

    public static float OutCubic(float t) {
      return 1f - Mathf.Pow(1f - t, 3f);
    }

    public static float OutQuart(float t) {
      return 1f - Mathf.Pow(1f - t, 4f);
    }

    public static float OutQuint(float t) {
      return 1f - Mathf.Pow(1f - t, 5f);
    }

    public static float OutExpo(float t) {
      return Mathf.Approximately(t, 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * t);
    }

    public static float OutCirc(float t) {
      return Mathf.Sqrt(1f - Mathf.Pow(t - 1f, 2f));
    }

    public static float OutBack(float t) {
      const float c1 = 1.70158f;
      const float c3 = c1 + 1f;

      return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    public static float OutElastic(float t) {
      const float c4 = 2f * Mathf.PI / 3f;
      return Mathf.Approximately(t, 0f)
        ? 0f
        : Mathf.Approximately(t, 1f)
          ? 1f
          : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
    }

    public static float OutBounce(float t) {
      const float n1 = 7.5625f;
      const float d1 = 2.75f;

      if (t < 1 / d1) {
        return n1 * t * t;
      }

      if (t < 2f / d1) {
        return n1 * (t -= 1.5f / d1) * t + 0.75f;
      }

      if (t < 2.5f / d1) {
        return n1 * (t -= 2.25f / d1) * t + 0.9375f;
      }

      return n1 * (t -= 2.625f / d1) * t + 0.984375f;
    }
  }
}
