using InGame.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InGame.GameConfiguration
{
  [HideMonoScript]
  [CreateAssetMenu(fileName = "Enemies", menuName = "Chronodeus/Configuration/Enemies")]
  public class EnemiesConfiguration : ScriptableObject
  {
    [SerializeField]
    [ListDrawerSettings(ShowFoldout = false, DefaultExpandedState = true)]
    [LabelText("Enemies list")]
    [InlineEditor(
      InlineEditorModes.GUIAndPreview,
      InlineEditorObjectFieldModes.Hidden,
      PreviewAlignment = PreviewAlignment.Left
    )]
    private Enemy[] list;

    public Enemy[] List => list;
  }
}
