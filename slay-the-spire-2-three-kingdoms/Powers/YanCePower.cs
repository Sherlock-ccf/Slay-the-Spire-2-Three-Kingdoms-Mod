using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Random;
using System.Runtime.Intrinsics.Arm;
namespace slay_the_spire_2_three_kingdoms.Powers;


public class YanCePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/YanCe.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/YanCe.png";
    private class Data
    {
        public CardType[] next = new CardType[3];
        public int cardPlayed, CorrectNumber;
    }
    protected override object InitInternalData()
    {
        return new Data();
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DynamicVar("NextCardOne", 0m),
        new DynamicVar("NextCardTwo", 0m),
        new DynamicVar("NextCardThree", 0m),
        new DynamicVar("CorrectNumber", 0m),
        new DynamicVar("Next", 0m)
    };
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player == null)
        {
            return Task.CompletedTask;
        }
        Rng rd = Owner.Player.RunState.Rng.CombatPotionGeneration;
        for (int i = 0; i <= 2; i++)
        {
            int num = rd.NextInt(2);
            if (i == 0)
            {
                DynamicVars["NextCardOne"].BaseValue = num;
            }
            if (i == 1)
            {
                DynamicVars["NextCardTwo"].BaseValue = num;
            }
            if (i == 2)
            {
                DynamicVars["NextCardThree"].BaseValue = num;
            }
            if (num == 0)
            {
                GetInternalData<Data>().next[i] = CardType.Attack;
            }
            else
            {
                GetInternalData<Data>().next[i] = CardType.Skill;
            }
        }
        GetInternalData<Data>().cardPlayed = 0;
        GetInternalData<Data>().CorrectNumber = 0;
        return Task.CompletedTask;
    }
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Owner.Player == null || cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        if (GetInternalData<Data>().cardPlayed == 0)
        {
            GetInternalData<Data>().cardPlayed++;
            DynamicVars["Next"].BaseValue = DynamicVars["NextCardOne"].BaseValue;
            return;
        }
        if (cardPlay.Card.Type == GetInternalData<Data>().next[GetInternalData<Data>().cardPlayed - 1])
        {
            GetInternalData<Data>().CorrectNumber++;
            DynamicVars["CorrectNumber"].BaseValue++;
            await CardPileCmd.Draw(context, 1, Owner.Player);
        }
        if (GetInternalData<Data>().cardPlayed <= 2)
        {
            DynamicVars["Next"].BaseValue = GetInternalData<Data>().next[GetInternalData<Data>().cardPlayed] == CardType.Attack ? 0 : 1;
        }
        GetInternalData<Data>().cardPlayed++;
        if (GetInternalData<Data>().cardPlayed >= 4)
        {
            switch (GetInternalData<Data>().CorrectNumber)
            {
                case 0:
                    await CreatureCmd.Damage(context, Owner, 6, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, Owner);
                    break;
                case 1:
                    await CardCmd.Discard(context, await CardSelectCmd.FromHandForDiscard(context, Owner.Player, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 1), null, this));
                    break;
                case 2:
                    await CardPileCmd.Draw(context, 1, Owner.Player);
                    break;
                case 3:
                    await CardPileCmd.Draw(context, 3, Owner.Player);
                    break;
                default: break;
            }
            await PowerCmd.Remove(this);
        }
    }

}