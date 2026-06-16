using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Combat;
namespace slay_the_spire_2_three_kingdoms.Powers;

public class DuanJinPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/DuanJin.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/DuanJin.png";
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Owner.Player == null || cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        CardModel card = cardPlay.Card;
        if (card.Rarity is CardRarity.Basic)
        {
            if (CombatState != null && CombatState.HittableEnemies.Count > 0)
            {
                Creature target = CombatState.HittableEnemies[0];
                await PowerCmd.Apply<WeakPower>(target, Amount, Owner.Player.Creature, null);
            }
        }
    }
}