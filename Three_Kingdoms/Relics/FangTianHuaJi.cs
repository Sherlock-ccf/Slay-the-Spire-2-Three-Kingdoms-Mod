using BaseLib.Abstracts;
using BaseLib.Utils;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Three_Kingdoms.Relics;

[Pool(typeof(TkRelicPool))]
public class FangTianHuaJi : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override string PackedIconPath => $"res://Three_Kingdoms/images/relics/FangTianHuaJi_sm.png";
    protected override string PackedIconOutlinePath => $"res://Three_Kingdoms/images/relics/FangTianHuaJi_sm.png";
    protected override string BigIconPath => $"res://Three_Kingdoms/images/relics/FangTianHuaJi_bg.png";

}
