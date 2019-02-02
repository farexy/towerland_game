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

    Stone = 6,
    Tree = 7
  }
}
