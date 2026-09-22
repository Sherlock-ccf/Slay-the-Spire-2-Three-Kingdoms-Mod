using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using slay_the_spire_2_three_kingdoms.Cards.Uncommon;
namespace slay_the_spire_2_three_kingdoms.Powers;


public class XieZhengPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/XieZheng.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/XieZheng.png";

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            CardModel cardModel = CombatState.CreateCard<BingLinChengXia>(Owner.Player);
            for (int i = 1; i <= Amount; i++)
            {
                if (Owner.Player.PlayerCombatState != null)
                {
                    CardModel card2 = cardModel.CreateClone();
                    await CardPileCmd.AddGeneratedCardToCombat(card2, Owner.Player.PlayerCombatState.Hand.Type, Owner.Player);
                }
            }
        }
    }
}