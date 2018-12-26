using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.Interfaces;
using Helpers;
using UnityEngine;

namespace Ai
{
  public class AiService
  {
    private readonly IStatsLibrary _statsLibrary;
    private readonly FieldManager _fieldManager;
    private readonly GameObjectType[] _availableMonsters;
    private readonly Dictionary<GameObjectType, int> _addedByType;
    private int _limit;

    public AiService(IStatsLibrary statsLibrary, FieldManager fieldManager)
    {
      _statsLibrary = statsLibrary;
      _fieldManager = fieldManager;
      _availableMonsters = _fieldManager.AvailableObjects
        .Where(t => GameObjectLogical.ResolveType(t) == GameObjectType.Unit).ToArray();
      _addedByType = _availableMonsters.ToDictionary(m => m, m => 0);
      _limit = 2;
    }

    public GameObjectType TryAddGameObject()
    {
      try
      {
        var available = _availableMonsters.Where(m => GetCost(m) <= _fieldManager.Field.State.MonsterMoney).ToArray();
        if (_addedByType.Values.Count > 0 && _addedByType.Values.All(v => v == _limit - 1))
        {
          _limit += 2;
        }

        if (GameMath.CalcProbableEvent(33))
        {
          var monsterToAdd = available.FirstOrDefault(m => GetCost(m) == available.Max(GetCost));
          if (monsterToAdd != GameObjectType.Undefined && _addedByType[monsterToAdd] < _limit)
          {
            _addedByType[monsterToAdd]++;
            return monsterToAdd;
          }
        }
      }
      catch (Exception ex)
      {
        Debug.LogError(ex);
      }

      return GameObjectType.Undefined;
    }

    private int GetCost(GameObjectType type)
    {
      return _statsLibrary.GetStats(type).Cost;
    }
  }
}