using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
namespace Three_Kingdoms.Powers;

public class SiZiPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string? CustomPackedIconPath => "res://Three_Kingdoms/images/powers/SiZi.png";
    public override string? CustomBigIconPath => "res://Three_Kingdoms/images/powers/SiZi.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DynamicVar("Bonus", 3m)
    };
    public void SetDamageBonus(bool upgraded)
    {
        DynamicVars["Bonus"].BaseValue = upgraded ? 6m : 3m;
    }
    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (dealer != Owner || target == Owner)
        {
            return 0m;
        }
        if (cardSource?.Type is not CardType.Attack || !props.IsPoweredAttack())
        {
            return 0m;
        }
        return DynamicVars["Bonus"].BaseValue;
    }
    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner?.Creature != Owner || card.Rarity != CardRarity.Basic)
        {
            return false;
        }
        if (originalCost <= 0m)
        {
            return false;
        }
        modifiedCost = originalCost - 1m;
        return true;
    }
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == Owner.Side)
        {
            await PowerCmd.TickDownDuration(this);
        }
    }
}
