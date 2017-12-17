using System;
using Assets.Scripts.Models.State;
using Newtonsoft.Json;

namespace Assets.Scripts.Network.Models
{
  public class BattleSearchCheckResponseModel
  {
    [JsonProperty("found")] public bool Found { get; set; }
    [JsonProperty("battleId")] public Guid BattleId { get; set; }
    [JsonProperty("side")] public PlayerSide Side { get; set; }
  }
}