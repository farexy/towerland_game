using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameActions;
using Assets.Scripts.Models.GameField;
using Helpers;
using UnityEngine;

namespace Assets.Scripts.Models.Resolvers
{
    public class ViewActionResolver : BaseActionResolver
    {
        private readonly FieldManager _fieldManager;
        
        public ViewActionResolver(FieldManager manager) : base(manager.Field)
        {
            _fieldManager = manager;
        }

        protected override void ResolveReservedAction(GameAction action)
        {
        }

        protected override void ResolveUnitAction(GameAction action)
        {
            switch (action.ActionId)
            {
                case ActionId.UnitMoves:
                    ManagersHelper.MonstersManager.MoveUnit(action.UnitId, action.Position, EffectId.None);
                    break;
                case ActionId.UnitMovesFreezed:
                    ManagersHelper.MonstersManager.MoveUnit(action.UnitId, action.Position, EffectId.UnitFreezed);
                    break;
                case ActionId.UnitFreezes:
                    _fieldManager.GetGameObjectById(action.UnitId).SetColor(Color.blue);
                    break;
                case ActionId.UnitEffectCanseled:
                    _fieldManager.GetGameObjectById(action.UnitId).SetColor(Color.clear);
                    break;
                case ActionId.UnitDies:
                    _fieldManager.RemoveGameObject(action.UnitId);
                    break;
                case ActionId.UnitAttacksCastle:
                    //logic
                    _fieldManager.RemoveGameObject(action.UnitId);
                    break;
            }
        }

        protected override void ResolveTowerAction(GameAction action)
        {
            switch (action.ActionId)
            {
                case ActionId.TowerAttacks:
                    var unitPos = _fieldManager.GetGameObjectById(action.UnitId).GetComponent<Rigidbody2D>().position;
                    ManagersHelper.TowerManager.ShowAttack(unitPos, action.TowerId);
                    break;
                case ActionId.TowerAttacksPosition:
                    ManagersHelper.TowerManager.ShowAttack(CoordinationHelper.GetViewPoint(action.Position), action.TowerId);
                    break;
            }      
        }

        protected override void ResolveOtherAction(GameAction action)
        {
        }
    }
}