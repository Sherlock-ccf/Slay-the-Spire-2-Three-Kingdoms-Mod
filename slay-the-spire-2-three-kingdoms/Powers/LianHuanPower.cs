using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using slay_the_spire_2_three_kingdoms.KeyWords;
using MegaCrit.Sts2.Core.Commands;
namespace slay_the_spire_2_three_kingdoms.Powers;

public class LianHuanPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/LianHuan.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/LianHuan.png";
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target.HasPower<LianHuanPower>() && result.UnblockedDamage > 0 && cardSource != null && cardSource.Keywords.Contains(TkKeywords.Fire))
        {
            foreach (Creature creature in CombatState.Creatures)
            {
                if (creature != target && creature.HasPower<LianHuanPower>())
                {
                    await PowerCmd.Remove<LianHuanPower>(creature);
                    await CreatureCmd.Damage(choiceContext, creature, result.UnblockedDamage, ValueProp.Unpowered, cardSource);
                }
            }
            await PowerCmd.Remove<LianHuanPower>(target);
        }
    }
}