using UnityEngine;

namespace InGame.GameConfiguration
{
  [CreateAssetMenu(fileName = "Configuration", menuName = "Chronodeus/Game Configuration")]
  public class Configuration : ScriptableObject
  {
    [SerializeField]
    private Map map;

    [SerializeField]
    private Player player;

    public Map Map => map;
    public Player Player => player;

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
