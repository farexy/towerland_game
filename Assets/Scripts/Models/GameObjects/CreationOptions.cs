using Assets.Scripts.Models.GameField;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.GameObjects
{
  public struct CreationOptions
  {
    [JsonProperty("p")] public Point Position;
    [JsonProperty("i")] public int? PathId; 
  }
}
