using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using slay_the_spire_2_three_kingdoms.Cards;
namespace slay_the_spire_2_three_kingdoms.Powers;


public class JueJinPower : CustomPowerModel
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;
    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Single;

    // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/JueJin.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/JueJin.png";

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            CardModel cardModel = CombatState.CreateCard<Jiu>(Owner.Player);
            CardCmd.ApplyKeyword(cardModel, CardKeyword.Exhaust);
            CardCmd.ApplyKeyword(cardModel, CardKeyword.Ethereal);
            cardModel.SetToFreeThisCombat();
            
            if (Owner.Player.PlayerCombatState != null)
            {
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, Owner.Player.PlayerCombatState.Hand.Type, Owner.Player);
            }
        }
    }
}