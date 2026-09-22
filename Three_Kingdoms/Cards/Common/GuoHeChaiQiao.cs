using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Common;

[Pool(typeof(TkCardPool))]
public class GuoHeChaiQiao : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(GuoHeChaiQiao)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(GuoHeChaiQiao)}.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("StrengthLoss", 1m)
    ];
    public GuoHeChaiQiao() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (cardPlay.Target != null)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, -DynamicVars["StrengthLoss"].BaseValue, Owner.Creature, this);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars["StrengthLoss"].UpgradeValueBy(2m);
    }
}