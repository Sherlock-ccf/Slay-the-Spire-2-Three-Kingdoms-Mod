using BaseLib.Abstracts;
using BaseLib.Utils;
using slay_the_spire_2_three_kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using slay_the_spire_2_three_kingdoms.KeyWords;
using MegaCrit.Sts2.GameInfo.Objects;

namespace slay_the_spire_2_three_kingdoms.Relics;

[Pool(typeof(TkRelicPool))]
public class TengJia : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Common;
    public override string PackedIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/TengJia_sm.png";
    protected override string PackedIconOutlinePath => $"res://slay_the_spire_2_three_kingdoms/images/relics/TengJia_sm.png";
    protected override string BigIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/TengJia_bg.png";
    public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress)
        {
            return amount;
        }
        if (!target.IsEnemy || cardSource == null || !cardSource.Keywords.Contains(TkKeywords.Fire))
        {
            return amount;
        }
        if (cardSource?.Owner == Owner || dealer == Owner.Creature)
        {
            return amount + 4;
        }
        return amount;
    }
}
