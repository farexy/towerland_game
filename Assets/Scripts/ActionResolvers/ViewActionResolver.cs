﻿using Assets.Scripts.Models.Client;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameActions;
using Assets.Scripts.Models.State;
using Helpers;
using UnityEngine;

namespace Assets.Scripts.Models.Resolvers
{
    public class ViewActionResolver : BaseActionResolver
    {
        private readonly FieldManager _fieldManager;
        private readonly MonstersManager _monstersManager;
        private readonly TowerManager _towerManager;

        public ViewActionResolver(FieldManager manager, MonstersManager monstersManager, TowerManager towerManager)
            : base(manager.Field)
        {
            _fieldManager = manager;
            _monstersManager = monstersManager;
            _towerManager = towerManager;
        }

        protected override void ResolveReservedAction(GameAction action)
        {
        }

        protected override void ResolveUnitAction(GameAction action)
        {
            switch (action.ActionId)
            {
                case ActionId.UnitMoves:
                    _monstersManager.MoveUnit(action.UnitId, action.Position, EffectId.None);
                    break;
                case ActionId.UnitGetsEffect:
                    //_fieldManager.GetGameObjectById(action.UnitId).SetEffect(action.EffectId);
                    break;
                case ActionId.UnitDisappears:
                    _fieldManager.RemoveGameObjectWithDelay(action.UnitId, 3);
                    break;
                case ActionId.UnitAttacksCastle:
                    _monstersManager.ShowAnimation(action.UnitId, MonsterAnimation.Attack);
                    break;
                case ActionId.UnitAppears:
                    _fieldManager.RenderFieldState();
                    break;
                case ActionId.UnitAppliesSkill:
                    _monstersManager.ShowAnimation(action.UnitId, MonsterAnimation.Skill);
                    break;
            }
        }

        protected override void ResolveTowerAction(GameAction action)
        {
            switch (action.ActionId)
            {
                case ActionId.TowerAttacks:
                    var unitPos = _fieldManager.GetGameObjectById(action.UnitId).GetComponent<Rigidbody2D>().position;
                    _towerManager.ShowTowerAttack(unitPos, action.TowerId);
                    break;
                case ActionId.TowerAttacksPosition:
                    _towerManager.ShowTowerAttack(CoordinationHelper.GetViewPoint(action.Position), action.TowerId);
                    break;
                case ActionId.TowerKills:
                    _monstersManager.ShowAnimation(action.UnitId, MonsterAnimation.Die);
                    break;
                case ActionId.TowerAppears:
                    _fieldManager.RenderFieldState();
                    break;
                case ActionId.TowerCollapses:
                    _towerManager.ShowCollapse(action.TowerId);
                    _fieldManager.RemoveGameObjectWithDelay(action.TowerId, 1.5f);
                    break;
                case ActionId.ShurikenAttacks:
                    var unit1Pos = _fieldManager.GetGameObjectById(action.UnitId).GetComponent<Rigidbody2D>().position;
                    var unit2Pos = _fieldManager.GetGameObjectById(action.UnitId2).GetComponent<Rigidbody2D>().position;
                    _towerManager.ShowShurikenAttack(unit1Pos, unit2Pos);
                    break;
            }
        }

        protected override void ResolveOtherAction(GameAction action)
        {
            switch (action.ActionId)
            {
                case ActionId.MonsterPlayerWins:
                    _fieldManager.End(PlayerSide.Monsters);
                    break;
                case ActionId.TowerPlayerWins:
                    _fieldManager.End(PlayerSide.Towers);
                    break;
            }
        }
    }
}