using System;
using Assets.Scripts.Models.State;

namespace Helpers
{
    public class LocalStorage
    {
        public static Guid PlayerId { get; set; }
        public static Guid CurrentBattleId { get; set; }
        public static PlayerSide CurrentSide { get; set; }
    }
}