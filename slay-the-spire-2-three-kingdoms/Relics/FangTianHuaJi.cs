using BaseLib.Abstracts;
using BaseLib.Utils;
using slay_the_spire_2_three_kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace slay_the_spire_2_three_kingdoms.Relics;

[Pool(typeof(TkRelicPool))]
public class FangTianHuaJi : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override string PackedIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/FangTianHuaJi_sm.png";
    protected override string PackedIconOutlinePath => $"res://slay_the_spire_2_three_kingdoms/images/relics/FangTianHuaJi_sm.png";
    protected override string BigIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/FangTianHuaJi_bg.png";
   
}
