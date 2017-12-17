using System.Collections.Generic;
using Assets.Scripts.Models.GameActions;
using Assets.Scripts.Models.GameField;
using Newtonsoft.Json;

namespace Assets.Scripts.Network.Models
{
    public class ActionsResponseModel
    {
        [JsonProperty("revision")]public int Revision { get; set; }
        [JsonProperty("state")]public FieldState State { get; set; }
        [JsonProperty("actionsByTick")]public IEnumerable<GameTick> ActionsByTicks { get; set; }
    }
}