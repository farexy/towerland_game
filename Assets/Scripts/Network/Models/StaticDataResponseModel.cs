using System;

namespace Assets.Scripts.Network.Models
{
  public class StaticDataResponseModel
  {
    public StatsResponseModel Stats { get; set; }
    public DateTime ServerTime { get; set; }
    public string ComputerPlayerSessionKey { get; set; }
  }
}