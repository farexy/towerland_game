using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.GameActions
{
  public class GameTick
  {
    [JsonProperty("t")] public DateTime RelativeTime { get; set; }
    [JsonProperty("a")] public IEnumerable<GameAction> Actions { get; set; }

    public bool HasNoActions => Actions == null || !Actions.Any();
  }
}
