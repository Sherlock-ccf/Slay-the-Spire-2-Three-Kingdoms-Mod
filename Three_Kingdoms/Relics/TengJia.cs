using BaseLib.Abstracts;
using BaseLib.Utils;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using Three_Kingdoms.KeyWords;

namespace Three_Kingdoms.Relics;

[Pool(typeof(TkRelicPool))]
public class TengJia : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Common;
    public override string PackedIconPath => $"res://Three_Kingdoms/images/relics/TengJia_sm.png";
    protected override string PackedIconOutlinePath => $"res://Three_Kingdoms/images/relics/TengJia_sm.png";
    protected override string BigIconPath => $"res://Three_Kingdoms/images/relics/TengJia_bg.png";
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
