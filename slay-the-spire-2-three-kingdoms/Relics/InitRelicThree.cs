using BaseLib.Abstracts;
using BaseLib.Utils;
using slay_the_spire_2_three_kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Entities.Cards;
using slay_the_spire_2_three_kingdoms.Cards.Basic;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Commands;

namespace slay_the_spire_2_three_kingdoms.Relics;
[Pool(typeof(TkRelicPool))]
public class InitRelicThree : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override string PackedIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_sm.png";
    protected override string PackedIconOutlinePath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_sm.png";
    protected override string BigIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_bg.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("AutoPlay",0)
    ];
    public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress)
        {
            return amount;
        }
        if (target != Owner.Creature || Owner.Creature.Player == null || cardSource?.Owner == Owner || dealer == Owner.Creature)
        {
            return amount;
        }
        if (cardSource != null && cardSource.Owner == Owner)
        {
            return amount;
        }
        if (amount < target.Block || amount == 0)
        {
            return amount;
        }
        if (!CombatManager.Instance.IsInProgress)
        {
            return amount;
        }
        if (target == Owner.Creature && Owner.Creature.Player != null && cardSource?.Owner != Owner && dealer != Owner.Creature)
        {
            IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(Owner.Creature.Player).Cards.Where((CardModel c) => c is Shan).ToList();
            if (enumerable != null)
            {
                foreach (CardModel item in enumerable)
                {
                    DynamicVars["AutoPlay"].BaseValue = 1;
                    return 0;
                }
            }
        }
        return amount;
    }
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress)
        {
            return;
        }
        if (target == Owner.Creature && Owner.Creature.Player != null && cardSource?.Owner != Owner && dealer != Owner.Creature)
        {
            IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(Owner.Creature.Player).Cards.Where((CardModel c) => c is Shan).ToList();
            if (enumerable != null && DynamicVars["AutoPlay"].BaseValue == 1)
            {
                foreach (CardModel item in enumerable)
                {
                    DynamicVars["AutoPlay"].BaseValue = 0;
                    await CardCmd.AutoPlay(choiceContext, item, null);
                    break;
                }
            }
        }

    }
    public override bool ShouldClearBlock(Creature creature)
    {
        if (Owner.Creature != creature)
        {
            return true;
        }
        return false;
    }
}
