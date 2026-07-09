using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using slay_the_spire_2_three_kingdoms.Character;
using slay_the_spire_2_three_kingdoms.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using slay_the_spire_2_three_kingdoms.Node;
namespace slay_the_spire_2_three_kingdoms.Cards;

[Pool(typeof(TkCardPool))]
public class LianHuan : CustomCardModel
{
	public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(LianHuan)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AllEnemies;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(LianHuan)}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> {
        HoverTipFactory.FromPower<LianHuanPower>(),
    };
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("WeakGive", 1m)
    ];
    public LianHuan() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (CombatState == null)
        {
            return;
        }
        await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 1), null, this));
        foreach (Creature creature in CombatState.Enemies)
        {
            if (!creature.HasPower<LianHuanPower>())
            {
                await PowerCmd.Apply<WeakPower>(choiceContext, creature, DynamicVars["WeakGive"].BaseValue, Owner.Creature, this);
                await PowerCmd.Apply<LianHuanPower>(choiceContext, creature, 1, Owner.Creature, this);
            }
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars["WeakGive"].UpgradeValueBy(1m);
    }
}
