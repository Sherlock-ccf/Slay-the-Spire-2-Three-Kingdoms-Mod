using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using slay_the_spire_2_three_kingdoms.Cards;
namespace slay_the_spire_2_three_kingdoms.Powers;


public class JianXiongPower : CustomPowerModel
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;
    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Single;

    // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/JianXiong.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/JianXiong.png";

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == Owner && result.UnblockedDamage > 0 && Owner.Player != null)
        {
            IEnumerable<CardModel> enumerable = PileType.Discard.GetPile(Owner.Player).Cards.Where((CardModel c) => c is Sha).ToList();
            if(enumerable!=null)
            {
                foreach (CardModel item in enumerable)
                {
                    await CardPileCmd.Add(item, PileType.Hand);
                    break;
                }
            }
        }

    }
}