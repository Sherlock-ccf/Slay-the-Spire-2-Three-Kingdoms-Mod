using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using Three_Kingdoms.Cards.Basic;
using Three_Kingdoms.Relics;
namespace Three_Kingdoms.Powers;

public class QingDaoPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomPackedIconPath => "res://Three_Kingdoms/images/powers/QingDao.png";
    public override string? CustomBigIconPath => "res://Three_Kingdoms/images/powers/QingDao.png";
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == Owner && result.UnblockedDamage > 0 && Owner.Player != null)
        {
            CardModel cardModel = CombatState.CreateCard<Shan>(Owner.Player);
            if (Owner.Player.PlayerCombatState != null)
            {
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, Owner.Player.PlayerCombatState.Hand.Type, Owner.Player);
            }
            await PowerCmd.Remove(this);
        }
    }
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            InitRelicOne? relic = Owner.Player.Relics.OfType<InitRelicOne>().FirstOrDefault();
            if (relic != null)
            {
                await relic.AddHandLimit(1);
            }
            await PowerCmd.Remove(this);
        }
    }
}