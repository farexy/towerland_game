using Assets.Scripts.Models.GameObjects;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.State
{
  public class UnitCreationOption
  {
    [JsonProperty("type")] public GameObjectType Type { set; get; }
  }
}

