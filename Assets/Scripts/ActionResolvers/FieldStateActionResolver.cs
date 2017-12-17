﻿using System.Threading;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameActions;
using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;
using Helpers;

namespace Assets.Scripts.Models.Resolvers
{
    public class FieldStateActionResolver : BaseActionResolver
    {
        public FieldStateActionResolver(Field filed) : base(filed)
        {
        }

        protected override void ResolveReservedAction(GameAction action)
        {
            throw new System.NotImplementedException();
        }

        protected override void ResolveUnitAction(GameAction action)
        {
            switch (action.ActionId)
            {
                case ActionId.UnitMoves:
                case ActionId.UnitMovesFreezed:
                    _field.MoveUnit(action.UnitId, action.Position, action.WaitTicks);
                    break;

                case ActionId.UnitAttacksCastle:
                    _field.RemoveGameObject(action.UnitId);
                    _field.State.Castle.Health -= action.Damage;
                    break;

                case ActionId.UnitRecievesDamage:
                    ((Unit)_field[action.UnitId]).Health -= action.Damage;
                    break;

                case ActionId.UnitFreezes:
                    _field[action.UnitId].Effect = new SpecialEffect{Duration = action.WaitTicks, Effect = EffectId.UnitFreezed};
                    break;
          
                case ActionId.UnitDies:
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        Thread.Sleep(500);
                        _field.RemoveGameObject(action.UnitId);
                    });
                    break;
          
                case ActionId.UnitEffectCanseled:
                    _field[action.UnitId].Effect = SpecialEffect.Empty;
                    break;
            }
        }

        protected override void ResolveTowerAction(GameAction action)
        {
            switch (action.ActionId)
            {
                case ActionId.TowerAttacks:
                case ActionId.TowerAttacksPosition:
                    _field[action.TowerId].WaitTicks = action.WaitTicks;
                    break;
            }
        }

        protected override void ResolveOtherAction(GameAction action)
        {
            switch (action.ActionId)
            {
                case ActionId.MonsterPlayerRecievesMoney:
                    _field.State.MonsterMoney += action.Money;
                    break;
                case ActionId.TowerPlayerRecievesMoney:
                    _field.State.TowerMoney += action.Money;
                    break;
            }
        }
    }
}