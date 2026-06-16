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
using slay_the_spire_2_three_kingdoms.Cards;
using MegaCrit.Sts2.Core.Commands;

namespace slay_the_spire_2_three_kingdoms.Relics;
// 加入哪个遗物池，此处为通用
[Pool(typeof(TkRelicPool))]
public class InitRelicThree : CustomRelicModel
{
    // 稀有度
    public override RelicRarity Rarity => RelicRarity.Starter;

    // 小图标（原版85x85）
    public override string PackedIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_sm.png";
    // 轮廓图标（原版85x85）
    protected override string PackedIconOutlinePath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_sm.png";
    // 大图标（原版256x256）
    protected override string BigIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_bg.png";

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
        bool hasShan = PileType.Hand.GetPile(Owner.Creature.Player)
        .Cards.Any(c => c is Shan);
        if (hasShan)
        {
            return 0m;
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
            if (enumerable != null)
            {
                foreach (CardModel item in enumerable)
                {
                    await CardCmd.AutoPlay(choiceContext, item, Owner.Creature);
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
