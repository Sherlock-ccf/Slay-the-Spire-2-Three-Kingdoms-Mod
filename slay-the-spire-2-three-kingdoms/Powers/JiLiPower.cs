using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
namespace slay_the_spire_2_three_kingdoms.Powers;


public class JiLiPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/JiLi.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/JiLi.png";
    private class Data
    {
        public int cardsPlayedThisTurn;
    }
    protected override object InitInternalData()
    {
        return new Data();
    }
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        GetInternalData<Data>().cardsPlayedThisTurn = 0;
        return Task.CompletedTask;
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DynamicVar("CardsPlayed", 0m),
        new DynamicVar("Amount", 0m)
    };
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Owner.Player == null || cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        if (DynamicVars["Amount"].BaseValue == 0)
        {
            DynamicVars["Amount"].BaseValue = Amount;
        }
        GetInternalData<Data>().cardsPlayedThisTurn++;
        DynamicVars["CardsPlayed"].BaseValue++;
        int tmp = GetInternalData<Data>().cardsPlayedThisTurn;
        if (tmp == Amount)
        {
            await CardPileCmd.Draw(context, tmp, Owner.Player);
        }
    }
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            GetInternalData<Data>().cardsPlayedThisTurn = 0;
            DynamicVars["CardsPlayed"].BaseValue = 0;
        }
    }
}