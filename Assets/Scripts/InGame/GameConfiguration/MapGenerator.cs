using System;
using InGame.Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.GameConfiguration
{
  public enum GeneratorType
  {
    Perlin,
    Range,
  }

  [Serializable]
  public class MapGenerator
  {
    [Title("Generator type")]
    [SerializeField]
    private GeneratorType type = GeneratorType.Perlin;

    [SerializeField]
    [LabelText("Object")]
    private MapObject mapObject;

    [Title("Perlin configuration")]
    [SerializeField]
    [Range(0f, 10f)]
    [ShowIf("type", GeneratorType.Perlin)]
    private float noise = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    [ShowIf("type", GeneratorType.Perlin)]
    private float density = 0.5f;

    [Title("Range configuration")]
    [SerializeField]
    [Range(0, 50)]
    [ShowIf("type", GeneratorType.Range)]
    private int amountFixed = 0;

    [SerializeField]
    [Range(0f, 1f)]
    [ShowIf("type", GeneratorType.Range)]
    [Tooltip("Percent based on the amount of total tiles on the map")]
    private float amountPercent = 0f;

    [SerializeField]
    [Range(0, 50)]
    [ShowIf("type", GeneratorType.Range)]
    private int padding = 0;

    public GeneratorType Type => type;
    public MapObject MapObject => mapObject;
    public float Noise => noise;
    public float Density => 1f - density;
    public int Padding => padding;
    public int AmountFixed => amountFixed;
    public float AmountPercent => amountPercent;
  }
}
