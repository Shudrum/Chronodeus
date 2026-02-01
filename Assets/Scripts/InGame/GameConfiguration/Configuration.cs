using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.GameConfiguration
{
  [HideMonoScript]
  [CreateAssetMenu(fileName = "Configuration", menuName = "Chronodeus/Game Configuration")]
  public class Configuration : ScriptableObject
  {
    [Title("Configuration files references")]
    [SerializeField]
    private MapConfiguration map;

    [SerializeField]
    private PlayerConfiguration player;

    [SerializeField]
    private EnemiesConfiguration enemies;

    public MapConfiguration Map => map;
    public PlayerConfiguration Player => player;
    public EnemiesConfiguration Enemies => enemies;

    private static bool _instanceLoaded;
    private static Configuration _instance;

    public static Configuration Instance {
      get {
        if (_instanceLoaded) return _instance;

        _instance = Resources.Load<Configuration>("Game Configuration");
        _instanceLoaded = true;
        return _instance;
      }
    }
  }
}
