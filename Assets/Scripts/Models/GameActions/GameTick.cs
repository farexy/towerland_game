using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Models.GameActions
{
  public class GameTick
  {
    public int RelativeTime { get; set; }
    public IEnumerable<GameAction> Actions { get; set; }

    public bool HasNoActions
    {
      get { return Actions == null || !Actions.Any(); }
    }
  }
}
