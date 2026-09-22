using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using Three_Kingdoms.Cards.Token;
namespace Three_Kingdoms.Powers;

public class DuanJinPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string? CustomPackedIconPath => "res://Three_Kingdoms/images/powers/DuanJin.png";
    public override string? CustomBigIconPath => "res://Three_Kingdoms/images/powers/DuanJin.png";
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Owner.Player == null || cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        CardModel card = cardPlay.Card;
        if (card.Rarity is CardRarity.Basic || card is HuoSha)
        {
            if (CombatState != null && CombatState.HittableEnemies.Count > 0)
            {
                Creature target = CombatState.HittableEnemies[0];
                await PowerCmd.Apply<WeakPower>(context, target, Amount, Owner.Player.Creature, null);
            }
        }
    }
}