using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.State
{
  public class TowerCreationOption
  {
    [JsonProperty("t")] public GameObjectType Type { set; get; }
    [JsonProperty("p")] public Point Position { set; get; }
  }
}
