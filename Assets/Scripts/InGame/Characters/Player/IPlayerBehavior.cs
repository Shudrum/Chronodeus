using Utils;

namespace InGame.Characters.Player
{
  public interface IPlayerBehavior
  {
    public void Initialize(StateMachine<PlayerState> state);

    public void UpdateBehavior();
  }
}
