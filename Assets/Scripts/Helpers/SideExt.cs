using System;
using Assets.Scripts.Models.State;

namespace Helpers
{
    public static class SideExt
    {
        public static PlayerSide Invert(this PlayerSide side)
        {
            switch (side)
            {
                case PlayerSide.Undefined:
                    return PlayerSide.Undefined;
                case PlayerSide.Monsters:
                    return PlayerSide.Towers;
                case PlayerSide.Towers:
                    return PlayerSide.Monsters;
                default:
                    throw new ArgumentOutOfRangeException("side", side, null);
            }
        }

        public static bool IsMonsters(this PlayerSide playerSide)
        {
            return playerSide == PlayerSide.Monsters;
        }
        
        public static bool IsTowers(this PlayerSide playerSide)
        {
            return playerSide == PlayerSide.Towers;
        }
    }
}