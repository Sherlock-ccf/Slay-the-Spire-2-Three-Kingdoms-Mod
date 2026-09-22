using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Three_Kingdoms.Character;
using Three_Kingdoms.Powers;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class KangKai : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(KangKai)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Power;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BlockGet", 4)];
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(KangKai)}.png";
    public KangKai() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await PowerCmd.Apply<KangKaiPower>(choiceContext, Owner.Creature, DynamicVars["BlockGet"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["BlockGet"].UpgradeValueBy(2);
    }
}
