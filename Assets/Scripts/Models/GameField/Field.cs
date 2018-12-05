using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.GameObjects;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.GameField
{
    public class Field : ICloneable
    {
        [JsonProperty("_id")] private int _objectId;

        [JsonProperty("sd")]
        public FieldStaticData StaticData { private set; get; }

        [JsonProperty("state")]
        public FieldState State { private set; get; }

        private Dictionary<int, GameObjectLogical> _objects;

        public Field()
        {
            _objectId = 1;
            _objects = new Dictionary<int, GameObjectLogical>();
            State = new FieldState();
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

        public GameObjectLogical this[int gameId]
        {
            get { return !_objects.ContainsKey(gameId) ? null : _objects[gameId]; }
        }


        public int AddGameObject(GameObjectLogical gameObj)
        {
            var id = gameObj.GameId == default(int)
                ? GenerateGameObjectId()
                : gameObj.GameId;
            return AddGameObject(id, gameObj);
        }

        private int AddGameObject(int gameId, GameObjectLogical gameObj)
        {
            var type = gameObj.ResolveType();
            gameObj.GameId = gameId;

            _objects.Add(gameId, gameObj);

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

            return gameId;
        }

        public IEnumerable<int> AddMany(IEnumerable<GameObjectLogical> objects)
        {
            return objects.Select(AddGameObject);
        }

        public int GenerateGameObjectId()
        {
            unchecked
            {
                return _objectId++;
            }
        }

        public void RemoveGameObject(int gameId)
        {
            if (!_objects.ContainsKey(gameId))
                throw new ArgumentException("There is no object with specified GameId on the field");

            var gameObj = _objects[gameId];
            var type = gameObj.ResolveType();

            switch (type)
            {
                case GameObjectType.Castle:
                    throw new ArgumentException("There can be only one castle");
                case GameObjectType.Unit:
                    State.Units.Remove((Unit)gameObj);
                    break;
                case GameObjectType.Tower:
                    State.Towers.Remove((Tower)gameObj);
                    break;
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
            _objects = state.Units.Cast<GameObjectLogical>().Union(state.Towers.Cast<GameObjectLogical>()).ToDictionary(o => o.GameId);
            this.State = new FieldState(_objects, state.Castle, state.TowerMoney, state.MonsterMoney);
        }

        public bool HasObject(int id)
        {
            return _objects.ContainsKey(id);
        }

        public object Clone()
        {
            var clonedObjects = _objects.ToDictionary(item => item.Key, item => (GameObjectLogical) item.Value.Clone());
            return new Field
            {
                StaticData = new FieldStaticData(StaticData.Cells, StaticData.Start, StaticData.Finish)
                {
                    Path = StaticData.Path,
                    EndTimeUtc = StaticData.EndTimeUtc
                },
                _objects = clonedObjects,
                State = new FieldState(clonedObjects, State.Castle, State.TowerMoney, State.MonsterMoney)
            };
        }
    }
}
