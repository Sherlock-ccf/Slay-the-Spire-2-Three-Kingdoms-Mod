using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Entities.Cards;
namespace slay_the_spire_2_three_kingdoms.Powers;


public class DangGuPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/DangGu.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/DangGu.png";
    private class Data
    {
        public int LastTurn;
    }
    protected override object InitInternalData()
    {
        return new Data();
    }
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        GetInternalData<Data>().LastTurn = 5;
        return Task.CompletedTask;
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DynamicVar("LastTurn", 5m),
    };
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            GetInternalData<Data>().LastTurn--;
            DynamicVars["LastTurn"].BaseValue--;
            await PlayerCmd.GainEnergy(Amount, player);
            CardModel? cardModel = CardFactory.GetDistinctForCombat
                (Owner.Player, from c in Owner.Player.Character.CardPool.GetUnlockedCards
                    (Owner.Player.UnlockState, Owner.Player.RunState.CardMultiplayerConstraint)
                               where c.Type == CardType.Power
                               select c, Amount, Owner.Player.RunState.Rng.CombatCardGeneration).FirstOrDefault();
            if (cardModel != null)
            {
                cardModel.SetToFreeThisCombat();
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, addedByPlayer: true);
            }
        }
    }
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side && GetInternalData<Data>().LastTurn <= 1)
        {
            await CreatureCmd.Kill(Owner, force: true);
        }
    }
}