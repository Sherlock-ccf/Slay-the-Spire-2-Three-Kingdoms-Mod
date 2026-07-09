using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using slay_the_spire_2_three_kingdoms.Character;
using slay_the_spire_2_three_kingdoms.Powers;
using slay_the_spire_2_three_kingdoms.Node;
using MegaCrit.Sts2.Core.HoverTips;
namespace slay_the_spire_2_three_kingdoms.Cards;

[Pool(typeof(TkCardPool))]
public class HanYong : CustomCardModel
{
    public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(HanYong)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(HanYong)}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    => new[] { HoverTipFactory.FromPower<RanPower>(), HoverTipFactory.FromPower<JiuPower>() };
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("RanAmount", 3m)
    ];
    public HanYong() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardPlayer.PlayCardSfx(SfxPath);
        await CardPileCmd.Draw(choiceContext, 2, Owner);
        await PlayerCmd.GainEnergy(1, Owner);
        await PowerCmd.Apply<RanPower>(choiceContext, Owner.Creature, DynamicVars["RanAmount"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<JiuPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["RanAmount"].UpgradeValueBy(-1m);
    }
}
