using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class KuRou : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(KuRou)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar("BlockGet",9m, ValueProp.Move),
    ];
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(KuRou)}.png";

    public KuRou() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);

        await CreatureCmd.Damage(choiceContext, Owner.Creature, 3, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, Owner.Creature);
        await CreatureCmd.GainBlock(Owner.Creature, (BlockVar)DynamicVars["BlockGet"], cardPlay);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["BlockGet"].UpgradeValueBy(3m);
    }
}
