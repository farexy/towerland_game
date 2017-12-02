using System;

namespace Assets.Scripts.Models.GameField
{
  [Flags]
  public enum FieldObject
  {
    Road = 0,
    Ground = 1,
    Entrance = 2,
    Castle = 3,

    Tree = 10
  }
}
