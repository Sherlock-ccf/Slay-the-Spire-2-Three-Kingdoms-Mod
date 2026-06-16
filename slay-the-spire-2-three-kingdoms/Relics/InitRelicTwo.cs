using BaseLib.Abstracts;
using BaseLib.Utils;
using slay_the_spire_2_three_kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace slay_the_spire_2_three_kingdoms.Relics;
// 加入哪个遗物池，此处为通用
[Pool(typeof(TkRelicPool))]
public class InitRelicTwo : CustomRelicModel
{
    // 稀有度
    public override RelicRarity Rarity => RelicRarity.Starter;

    // 小图标（原版85x85）
    public override string PackedIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_sm.png";
    // 轮廓图标（原版85x85）
    protected override string PackedIconOutlinePath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_sm.png";
    // 大图标（原版256x256）
    protected override string BigIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_bg.png";

    public override bool ShouldFlush(Player player)
    {
        if (player != Owner)
        {
            return true;
        }
        return false;
    }
    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner)
        {
            return count;
        }
        if (player.Creature.CombatState != null && player.Creature.CombatState.RoundNumber > 1)
        {
            return count - 3;
        }
        return count + 1;
    }
}
