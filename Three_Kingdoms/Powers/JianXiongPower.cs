using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using Three_Kingdoms.Cards.Basic;
namespace Three_Kingdoms.Powers;


public class JianXiongPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override string? CustomPackedIconPath => "res://Three_Kingdoms/images/powers/JianXiong.png";
    public override string? CustomBigIconPath => "res://Three_Kingdoms/images/powers/JianXiong.png";

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