using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
using Three_Kingdoms.Powers;
namespace Three_Kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class SiZi : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(SiZi)}.mp3";
    protected override bool HasEnergyCostX => true;
    private const int energyCost = 0;
    private const CardType type = CardType.Power;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Bonus", 3m)];
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(SiZi)}.png";
    public SiZi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        int num = ResolveEnergyXValue();
        SiZiPower? power = await PowerCmd.Apply<SiZiPower>(choiceContext, Owner.Creature, num, Owner.Creature, this);
        power?.SetDamageBonus(IsUpgraded);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["Bonus"].UpgradeValueBy(3m);
    }
}
