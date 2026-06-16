using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
namespace slay_the_spire_2_three_kingdoms.Powers;

public class QinZhengPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/QinZheng.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/QinZheng.png";

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
        new DynamicVar("CardsPlayed", 0m)
    };
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Owner.Player == null || cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        GetInternalData<Data>().cardsPlayedThisTurn++;
        DynamicVars["CardsPlayed"].BaseValue++;
        int tmp = GetInternalData<Data>().cardsPlayedThisTurn;
        int cnt = 0;
        if (tmp % 3 == 0) { cnt++; }
        if (tmp % 5 == 0) { cnt++; }
        if (tmp % 8 == 0) { cnt++; }
        await CardPileCmd.Draw(context, cnt, Owner.Player);
    }
}