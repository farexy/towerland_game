using System;

namespace Helpers
{
  public class ServerTime
  {
    private static TimeSpan _offset;

    public static void Init(DateTime serverTime)
    {
      _offset = DateTime.UtcNow - serverTime;
    }

    public static DateTime Now => DateTime.Now - _offset;
  }
}