using System;

namespace InGame.Characters.Player
{
  [Flags]
  public enum PlayerState
  {
    Grounded = 1,
    // 2,
    Jumping = 4,
    Attacking = 8,
    Hauling = 16,
    IsInvincible = 32,
  }
}
