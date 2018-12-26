using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Helpers
{
  public static class SerializationExt
  {
    public static string ToJson(this object obj)
    {
      return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore
      });
    }
  }
}