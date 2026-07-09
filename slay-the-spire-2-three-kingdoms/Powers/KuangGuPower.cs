using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Entities.Creatures;
using slay_the_spire_2_three_kingdoms.Cards;
namespace slay_the_spire_2_three_kingdoms.Powers;

public class KuangGuPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DynamicVar("StrengthGet", 0m)
    };
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/KuangGu.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/KuangGu.png";

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Owner.Player == null || cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        CardModel card = cardPlay.Card;
        if (card is Sha || card is HuoSha)
        {
            await PowerCmd.Apply<StrengthPower>(context, Owner, 1, Owner, null, silent: true);
            DynamicVars["StrengthGet"].BaseValue++;
            await PlayerCmd.GainEnergy(1m, Owner.Player);
            await CreatureCmd.Heal(Owner.Player.Creature, 8);
            await CardPileCmd.Draw(context, 1, Owner.Player);
        }
    }
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == Owner.Side)
        {
            await PowerCmd.Remove(this);
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, -DynamicVars["StrengthGet"].BaseValue, Owner, null, silent: true);
        }
    }
}