using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Common;

[Pool(typeof(TkCardPool))]
public class TuXi : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(TuXi)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AllEnemies;
    private const bool shouldShowInCardLibrary = true;


    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(TuXi)}.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("StrengthLoss", 1m)
    ];
    public TuXi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 2), null, this));
        if (CombatState != null)
        {
            foreach (Creature enermy in CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<StrengthPower>(choiceContext, enermy, -DynamicVars["StrengthLoss"].BaseValue, Owner.Creature, this);
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["StrengthLoss"].UpgradeValueBy(1m);
    }
}
