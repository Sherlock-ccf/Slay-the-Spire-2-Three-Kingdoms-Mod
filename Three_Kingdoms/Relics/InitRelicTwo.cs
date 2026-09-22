using BaseLib.Abstracts;
using BaseLib.Utils;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Three_Kingdoms.Relics;
[Pool(typeof(TkRelicPool))]
public class InitRelicTwo : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override string PackedIconPath => $"res://Three_Kingdoms/images/relics/InitRelicOne_sm.png";
    protected override string PackedIconOutlinePath => $"res://Three_Kingdoms/images/relics/InitRelicOne_sm.png";
    protected override string BigIconPath => $"res://Three_Kingdoms/images/relics/InitRelicOne_bg.png";

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
