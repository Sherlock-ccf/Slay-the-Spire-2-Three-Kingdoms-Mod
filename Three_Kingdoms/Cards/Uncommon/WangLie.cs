using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class WangLie : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(WangLie)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(WangLie)}.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(9m,ValueProp.Move),
        new CardsVar(2)
    ];
    public WangLie() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await CreatureCmd.Damage(choiceContext, Owner.Creature, 3, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        if (cardPlay.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        }
        await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, Owner.Creature, 2m, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
