using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.GameObjects;

namespace Assets.Scripts.Models.GameField
{
   public class Field : ICloneable
  {
    public FieldStaticData StaticData { private set; get; }

    private FieldState _state;
    public FieldState State
    {
      get
      {
        _state.Objects = _objects;
        return _state;
      }
    }

    private Dictionary<int, GameObjectLogical> _objects;

    protected Field()
    {
    }

    public Field(FieldCell[,] cells)
    {
      _objects = new Dictionary<int, GameObjectLogical>();
      StaticData = new FieldStaticData(cells, 
        cells.Cast<FieldCell>().First(c => c.Object == FieldObject.Entrance).Position,
        cells.Cast<FieldCell>().First(c => c.Object == FieldObject.Castle).Position
        );
      _state = new FieldState();
    }

    public int AddGameObject(GameObjectLogical gameObj)
    {
      var type = gameObj.ResolveType();
      var id = _objects.Count + gameObj.GetHashCode() - _objects.LastOrDefault().Key;
      gameObj.GameId = id;

      switch (type)
      {
        case GameObjectType.Castle:
          if(State.Castle != null)
            throw new ArgumentException("There can be only one castle");
          State.Castle = (Castle) gameObj;
          break;
        case GameObjectType.Unit:
          State.Units.Add((Unit)gameObj);
          break;
        case GameObjectType.Tower:
          State.Towers.Add((Tower)gameObj);
          break;
      }

      _objects.Add(id, gameObj);

      return id;
    }

    public void AddMany(List<Tower> objects, List<Unit> objects1)
    {
      _objects.Clear();
      foreach (var o in objects)
      {
        _objects.Add(o.GameId, o);
      }
      foreach (var o1 in objects1)
      {
        _objects.Add(o1.GameId, o1);
      }
    }

    public void RemoveGameObject(int gameId)
    {
      if(!_objects.ContainsKey(gameId))
        throw new ArgumentException("There is no object with spisified GameId on the field");

      var gameObj = _objects[gameId];
      var type = gameObj.ResolveType();

      if (GameObjectType.Castle == type)
      {
        throw new ArgumentException("There can be only one castle");
      }
      if (type == GameObjectType.Unit)
      {
        State.Units.Remove(State.Units.First(u => u.GameId == gameId));
      }
      if (type == GameObjectType.Tower)
      {
        State.Towers.Remove(State.Towers.First(t => t.GameId == gameId));
      }

      _objects.Remove(gameId);
    }

    public void RemoveMany(IEnumerable<int> gameIds)
    {
      foreach (var id in gameIds)
      {
        RemoveGameObject(id);
      }
    }

    public GameObjectLogical this[int gameId]
    {
      get { return _objects[gameId]; }
    }

    public IEnumerable<GameObjectLogical> FindGameObjects(Predicate<GameObjectLogical> predicate)
    {
      return _objects.Values.Where(obj => predicate(obj));
    }

    public void SetState(FieldState state)
    {
      this._objects = state.Objects.ToDictionary(item => item.Key, item => item.Value);
      this._state = new FieldState(state.Towers, state.Units, state.Castle)
      {
        MonsterMoney = state.MonsterMoney,
        TowerMoney = state.TowerMoney,
      };
    }
    
    public object Clone()
    {
      return new Field
      {
        StaticData = new FieldStaticData(StaticData.Cells, StaticData.Start, StaticData.Finish)
        {
          Path = StaticData.Path
        },
        _objects = _objects.ToDictionary(item => item.Key, item => item.Value),
        _state = new FieldState(State.Towers, State.Units, State.Castle)
        {
          Castle = new Castle {Health = State.Castle.Health, Position = State.Castle.Position},
          MonsterMoney = State.MonsterMoney,
          TowerMoney = State.TowerMoney
        },
      };
    }
  }
}
