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


public class JianYingPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/JianYing.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/JianYing.png";
    private class Data
    {
        public CardType CardPlayedType;
    }
    protected override object InitInternalData()
    {
        return new Data();
    }
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        GetInternalData<Data>().CardPlayedType = CardType.Power;

        return Task.CompletedTask;
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DynamicVar("CardPlayedType",3)
    };
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Owner.Player == null || cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        if (cardPlay.Card.Type != GetInternalData<Data>().CardPlayedType)
        {
            GetInternalData<Data>().CardPlayedType = cardPlay.Card.Type;
            DynamicVars["CardPlayedType"].BaseValue = (int)cardPlay.Card.Type;
            await CardPileCmd.Draw(context, Owner.Player);
        }
    }
}