using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Three_Kingdoms.KeyWords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
namespace Three_Kingdoms.Powers;

public class ChengHaoPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string? CustomPackedIconPath => "res://Three_Kingdoms/images/powers/ChengHao.png";
    public override string? CustomBigIconPath => "res://Three_Kingdoms/images/powers/ChengHao.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromKeyword(TkKeywords.Fire), HoverTipFactory.FromPower<LianHuanPower>() };
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target.HasPower<LianHuanPower>() && result.UnblockedDamage > 0 && cardSource != null && cardSource.Keywords.Contains(TkKeywords.Fire) && Owner.Player != null)
        {
            await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
        }
    }
}