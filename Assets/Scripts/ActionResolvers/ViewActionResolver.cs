using Assets.Scripts.Models.Client;
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
                case ActionId.UnitFreezes:
                    //_fieldManager.GetGameObjectById(action.UnitId).SetColor(Color.blue);
                    break;
                case ActionId.UnitEffectCanseled:
                    //_fieldManager.GetGameObjectById(action.UnitId).SetColor(Color.clear);
                    break;
                case ActionId.UnitDisappears:
                    _fieldManager.RemoveGameObjectWithDelay(action.UnitId, FieldManager.TickSecond * 3);
                    break;
                case ActionId.UnitAttacksCastle:
                    _monstersManager.ShowAnimation(action.UnitId, MonsterAnimation.Attack);
                    break;
                case ActionId.UnitAppears:
                    _fieldManager.RenderFieldState();
                    break;
                case ActionId.UnitAppliesEffect_DarkMagic:
                    // unit animation
                    break;
            }
        }

        protected override void ResolveTowerAction(GameAction action)
        {
            switch (action.ActionId)
            {
                case ActionId.TowerAttacks:
                    var unitPos = _fieldManager.GetGameObjectById(action.UnitId).GetComponent<Rigidbody2D>().position;
                    _towerManager.ShowAttack(unitPos, action.TowerId);
                    break;
                case ActionId.TowerAttacksPosition:
                    _towerManager.ShowAttack(CoordinationHelper.GetViewPoint(action.Position), action.TowerId);
                    break;
                case ActionId.TowerKills:
                    _monstersManager.ShowAnimation(action.UnitId, MonsterAnimation.Die);
                    break;
                case ActionId.TowerCollapses:
                    _fieldManager.RemoveGameObjectWithDelay(action.TowerId, FieldManager.TickSecond);
                    break;
            }
        }

        protected override void ResolveOtherAction(GameAction action)
        {
            switch (action.ActionId)
            {
                case ActionId.MonsterPlayerWins:
                    _monstersManager.ShowAnimation(MonsterAnimation.Victory);
                    _fieldManager.End(PlayerSide.Monsters);
                    break;
                case ActionId.TowerPlayerWins:
                    _fieldManager.End(PlayerSide.Towers);
                    break;
            }
        }
    }
}