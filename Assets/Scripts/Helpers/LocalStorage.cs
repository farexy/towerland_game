using System;
using Assets.Scripts.Models.Interfaces;
using Assets.Scripts.Models.State;

namespace Helpers
{
    public class LocalStorage
    {
        public static string Session { get; set; }
        public static string HelpSession { get; set; }
        public static Guid CurrentBattleId { get; set; }
        public static PlayerSide CurrentSide { get; set; }
        public static IStatsLibrary StatsLibrary { get; set; }
    }
}