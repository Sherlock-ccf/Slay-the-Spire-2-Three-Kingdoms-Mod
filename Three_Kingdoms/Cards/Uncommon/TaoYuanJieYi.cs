using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Entities.Creatures;
using Three_Kingdoms.Character;

using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;
[Pool(typeof(TkCardPool))]
public class TaoYuanJieYi : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(TaoYuanJieYi)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override bool CanBeGeneratedInCombat => false;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(TaoYuanJieYi)}.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new HealVar(3m)];
    public TaoYuanJieYi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
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
