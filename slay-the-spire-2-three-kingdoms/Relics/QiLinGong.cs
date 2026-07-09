using BaseLib.Abstracts;
using BaseLib.Utils;
using slay_the_spire_2_three_kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace slay_the_spire_2_three_kingdoms.Relics;

[Pool(typeof(TkRelicPool))]
public class QiLinGong : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Common;
    public override string PackedIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/QiLinGong_sm.png";
    protected override string PackedIconOutlinePath => $"res://slay_the_spire_2_three_kingdoms/images/relics/QiLinGong_sm.png";
    protected override string BigIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/QiLinGong_bg.png";
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if ((dealer == Owner.Creature || dealer?.PetOwner == Owner) && !target.IsPlayer && result.WasBlockBroken)
        {
            Flash();
            await PowerCmd.Apply<WeakPower>(choiceContext, target, 3m, Owner.Creature, null);
        }
    }
}
