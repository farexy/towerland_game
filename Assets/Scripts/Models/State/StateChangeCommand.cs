using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.State
{
  public class StateChangeCommand
  {
    public CommandId Id { set; get; }
    public Guid BattleId { set; get; }
    public IEnumerable<UnitCreationOption> UnitCreationOptions { set; get; }
    public IEnumerable<TowerCreationOption> TowerCreationOptions { set; get; }
  }
}
