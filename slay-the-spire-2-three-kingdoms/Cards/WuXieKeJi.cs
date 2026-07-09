using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using slay_the_spire_2_three_kingdoms.Character;
using MegaCrit.Sts2.Core.Models.Powers;
using slay_the_spire_2_three_kingdoms.Node;
namespace slay_the_spire_2_three_kingdoms.Cards;

// 加入哪个卡池
[Pool(typeof(TkCardPool))]
public class WuXieKeJi : CustomCardModel
{
	public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(WuXieKeJi)}.mp3";
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

    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(WuXieKeJi)}.png";

    public WuXieKeJi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await PowerCmd.Apply<ArtifactPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
