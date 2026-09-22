using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class ShunShouQianYang : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(ShunShouQianYang)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;


    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(ShunShouQianYang)}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    => new[] { HoverTipFactory.FromPower<StrengthPower>() };
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("StrengthLoss",1m),
        new DynamicVar("StrengthGet",1m)
    ];
    public ShunShouQianYang() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (cardPlay.Target != null)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, -DynamicVars["StrengthLoss"].BaseValue, Owner.Creature, this);
        }
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars["StrengthGet"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["StrengthLoss"].UpgradeValueBy(1m);
        DynamicVars["StrengthGet"].UpgradeValueBy(1m);
    }
}
