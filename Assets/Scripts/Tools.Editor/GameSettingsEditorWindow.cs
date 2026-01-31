using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Tools.Editor
{
  public class GameSettingsEditorWindow : OdinMenuEditorWindow
  {
    [MenuItem("Chronodeus/Game Settings", priority = 1)]
    private static void OpenWindow() {
      GetWindow<GameSettingsEditorWindow>("Game Settings").Show();
    }

    protected override OdinMenuTree BuildMenuTree() {
      var tree = new OdinMenuTree { Selection = { SupportsMultiSelect = false } };

      tree.AddAllAssetsAtPath(
        "Settings",
        "Assets/Settings/Configuration",
        typeof(ScriptableObject),
        true,
        true
      );

      return tree;
    }
  }
}
