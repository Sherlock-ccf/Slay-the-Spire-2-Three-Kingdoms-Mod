using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
namespace Three_Kingdoms.Powers;
public class ZhiHengPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomPackedIconPath => "res://Three_Kingdoms/images/powers/ZhiHeng.png";
    public override string? CustomBigIconPath => "res://Three_Kingdoms/images/powers/ZhiHeng.png";
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if(player==Owner.Player)
        {
            List<CardModel> list = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 0, 999999999), context: choiceContext, player: player, filter: null, source: this)).ToList();
            if (list.Count != 0)
            {
                await CardCmd.DiscardAndDraw(choiceContext, list, list.Count);
            }
        }
    }
}