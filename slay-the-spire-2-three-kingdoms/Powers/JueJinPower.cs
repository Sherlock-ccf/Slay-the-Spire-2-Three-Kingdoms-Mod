using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using slay_the_spire_2_three_kingdoms.Cards.Basic;
namespace slay_the_spire_2_three_kingdoms.Powers;


public class JueJinPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

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