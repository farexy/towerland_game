﻿using System.Threading;
using System.Threading.Tasks;
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
                    _field.MoveUnit(action.UnitId, action.Position, action.WaitTicks);
                    break;

                case ActionId.UnitAttacksCastle:
                    _field.State.Castle.Health -= action.Damage;
                    break;

                case ActionId.UnitReceivesDamage:
                    ((Unit)_field[action.UnitId]).Health -= action.Damage;
                    break;

                case ActionId.UnitGetsEffect:
                    _field[action.UnitId].Effect = new SpecialEffect{Id = action.EffectId, Duration = action.WaitTicks, EffectValue = action.EffectValue};
                    break;

                case ActionId.UnitEffectCanceled:
                    _field[action.UnitId].Effect = SpecialEffect.Empty;
                    break;

                case ActionId.UnitDisappears:
                    Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        _field.RemoveGameObject(action.UnitId);
                    });
                    break;

                case ActionId.UnitAppears:
                    _field.AddGameObject(action.GoUnit);
                    break;

                case ActionId.UnitAppliesSkill:
                    _field[action.UnitId].WaitTicks += action.WaitTicks;
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

                case ActionId.TowerGetsEffect:
                    _field[action.TowerId].Effect = new SpecialEffect{Id = action.EffectId, Duration = action.WaitTicks, EffectValue = action.EffectValue};
                    break;

                case ActionId.TowerEffectCanceled:
                    _field[action.TowerId].Effect = SpecialEffect.Empty;
                    break;

                case ActionId.TowerAppears:
                    _field.AddGameObject(action.GoTower);
                    break;

                case ActionId.TowerCollapses:
                    _field.RemoveGameObject(action.TowerId);
                    break;
            }
        }

        protected override void ResolveOtherAction(GameAction action)
        {
            switch (action.ActionId)
            {
                case ActionId.MonsterPlayerReceivesMoney:
                    _field.State.MonsterMoney += action.Money;
                    break;
                case ActionId.TowerPlayerReceivesMoney:
                    _field.State.TowerMoney += action.Money;
                    break;
                case ActionId.PlayersReceivesMoney:
                    _field.State.TowerMoney += action.Money;
                    _field.State.MonsterMoney += action.Money;
                    break;
                case ActionId.TowerPlayerLosesMoney:
                    _field.State.TowerMoney -= action.Money;
                    break;
                case ActionId.MonsterPlayerLosesMoney:
                    _field.State.MonsterMoney -= action.Money;
                    break;
            }
        }
    }
}