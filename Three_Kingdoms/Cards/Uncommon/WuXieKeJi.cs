using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Models.Powers;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class WuXieKeJi : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(WuXieKeJi)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    => new[] { HoverTipFactory.FromPower<ArtifactPower>() };
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ArtifactPower>(1m),
    ];

    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(WuXieKeJi)}.png";

    public WuXieKeJi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await PowerCmd.Apply<ArtifactPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
