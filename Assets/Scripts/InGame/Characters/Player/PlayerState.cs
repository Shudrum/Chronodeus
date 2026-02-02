using System;

namespace InGame.Characters.Player
{
  [Flags]
  public enum PlayerState
  {
    Grounded = 1,
    IsInvincible = 2,
    Jumping = 4,
    Attacking = 8,
    HaulingAnimated = 16,
    Hauling = 32,
  }
}
