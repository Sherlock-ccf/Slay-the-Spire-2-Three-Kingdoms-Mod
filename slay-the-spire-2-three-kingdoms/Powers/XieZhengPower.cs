using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using slay_the_spire_2_three_kingdoms.Cards;
namespace slay_the_spire_2_three_kingdoms.Powers;


public class XieZhengPower : CustomPowerModel
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;
    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Counter;

    // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
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