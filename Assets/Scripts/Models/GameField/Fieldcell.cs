using Newtonsoft.Json;

namespace Assets.Scripts.Models.GameField
{
  public struct FieldCell
  {
    [JsonProperty("o")] public FieldObject Object;
    [JsonProperty("p")] public Point Position;
  }
}
