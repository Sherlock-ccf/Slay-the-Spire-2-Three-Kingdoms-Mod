using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Basic;

[Pool(typeof(TkCardPool))]
public class Tao : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(Tao)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Basic;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(6m)];
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(Tao)}.png";
    public Tao() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await CreatureCmd.Heal(
        Owner.Creature,
        DynamicVars.Heal.BaseValue
        );
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
