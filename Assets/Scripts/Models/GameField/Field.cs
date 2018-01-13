﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.GameObjects;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.GameField
{
   public class Field : ICloneable
    {
        public GameObjectLogical this[int gameId]
        {
            get { return _objects[gameId]; }
        }

        [JsonProperty("sd")]
        public FieldStaticData StaticData { private set; get; }

        [JsonProperty("state")]
        private FieldState _state;
        [JsonIgnore]
        public FieldState State
        {
            get
            {
                _state.Objects = _objects;
                return _state;
            }
        }

        private Dictionary<int, GameObjectLogical> _objects;

        public Field()
        {
            _objects = new Dictionary<int, GameObjectLogical>();
            _state = new FieldState();
        }

        public Field(FieldStaticData staticData) : this()
        {
            StaticData = staticData;
        }

        public Field(FieldCell[,] cells) : this()
        {
            StaticData = new FieldStaticData(cells,
                cells.Cast<FieldCell>().First(c => c.Object == FieldObject.Entrance).Position,
                cells.Cast<FieldCell>().First(c => c.Object == FieldObject.Castle).Position
            );
        }

        public int AddGameObject(GameObjectLogical gameObj)
        {
            var type = gameObj.ResolveType();
            var id = _objects.Count + gameObj.GetHashCode() - _objects.LastOrDefault().Key;
            gameObj.GameId = id;

            switch (type)
            {
                case GameObjectType.Castle:
                    if (State.Castle != null)
                        throw new ArgumentException("There can be only one castle");
                    State.Castle = (Castle)gameObj;
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

        public IEnumerable<int> AddMany(IEnumerable<GameObjectLogical> objects)
        {
            return objects.Select(AddGameObject);
        }

        public void RemoveGameObject(int gameId)
        {
            if (!_objects.ContainsKey(gameId))
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

        public IEnumerable<GameObjectLogical> FindGameObjects(Predicate<GameObjectLogical> predicate)
        {
            return _objects.Values.Where(obj => predicate(obj));
        }

        public void SetState(FieldState state)
        {
            this._state = new FieldState(state.Towers, state.Units, state.Castle)
            {
                MonsterMoney = state.MonsterMoney,
                TowerMoney = state.TowerMoney
            };
            this._objects = SetObjects(_state.Towers, _state.Units);
        }

        public object Clone()
        {
            return new Field
            {
                StaticData = new FieldStaticData(StaticData.Cells, StaticData.Start, StaticData.Finish)
                {
                    Path = StaticData.Path
                },
                _objects = _objects.ToDictionary(item => item.Key, item => (GameObjectLogical)item.Value.Clone()),
                _state = new FieldState(State.Towers, State.Units, State.Castle)
                {
                    MonsterMoney = State.MonsterMoney,
                    TowerMoney = State.TowerMoney
                },
            };
        }
        
        private static Dictionary<int, GameObjectLogical> SetObjects(List<Tower> objects, List<Unit> objects1)
        {
            var res = new Dictionary<int, GameObjectLogical>();
            foreach (var o in objects)
            {
                res.Add(o.GameId, o);
            }
            foreach (var o1 in objects1)
            {
                res.Add(o1.GameId, o1);
            }
            return res;
        }
    }
}
