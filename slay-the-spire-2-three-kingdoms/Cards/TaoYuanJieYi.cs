using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Entities.Creatures;
using slay_the_spire_2_three_kingdoms.Character;

namespace slay_the_spire_2_three_kingdoms.Cards;
[Pool(typeof(TkCardPool))]
public class TaoYuanJieYi : CustomCardModel
{
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override bool CanBeGeneratedInCombat => false;
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(TaoYuanJieYi)}.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new HealVar(3m)];
    public TaoYuanJieYi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        if (CombatState != null)
        {
            foreach (Creature target in CombatState.HittableEnemies)
            {
                await CreatureCmd.Heal(target, DynamicVars.Heal.BaseValue);
            }
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
        DynamicVars.Heal.UpgradeValueBy(2);
    }
}
