using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.Interfaces;

namespace Assets.Scripts.Models.Stats
{
    public class StatsLibrary : IStatsLibrary
    {
        private readonly Dictionary<GameObjectType, IStats> _objects;
        private readonly IEnumerable<DefenceCoeff> _deffCoeffs;
        private readonly IEnumerable<Skill> _skills;
      
        public StatsLibrary(UnitStats[] units, TowerStats[] towers, DefenceCoeff[] defenceCoeffs, Skill[] skills)
        {
            _objects = towers
                .Cast<IStats>()
                .Union(units.Cast<IStats>())
                .ToDictionary(el => el.Type, el => el);
            _deffCoeffs = defenceCoeffs;
            _skills = skills;
        }

        public IStats GetStats(GameObjectType type)
        {
            return _objects[type];
        }
        
        public UnitStats GetUnitStats(GameObjectType type)
        {
            return (UnitStats) _objects[type];
        }

        public TowerStats GetTowerStats(GameObjectType type)
        {
            return (TowerStats) _objects[type];
        }

        public double GetDefenceCoeff(UnitStats.DefenceType defType, TowerStats.AttackType attackType)
        {
            return _deffCoeffs
                .First(defCoeff => defCoeff.Attack == attackType && defCoeff.Defence == defType)
                .Coeff;
        }

        public Skill GetSkill(SkillId id, GameObjectType goType)
        {
            try
            {
                return _skills.First(skill => skill.Id == id && skill.GameObjectType == goType);
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException($"Skill {id} for game object {goType} is not found");
            }
        }
    }
}