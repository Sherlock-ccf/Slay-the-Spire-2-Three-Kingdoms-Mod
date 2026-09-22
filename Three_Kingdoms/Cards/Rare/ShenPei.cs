using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Entities.Players;
using Three_Kingdoms.KeyWords;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class ShenPei : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(ShenPei)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(ShenPei)}.png";
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust, TkKeywords.Fire };
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("HpLost",0m),
    ];
    public ShenPei() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);

        if (cardPlay.Target != null)
        {
            await DamageCmd.Attack(DynamicVars["HpLost"].BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        }
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == Owner.Creature && result.UnblockedDamage > 0 && Owner != null)
        {
            DynamicVars["HpLost"].BaseValue += result.UnblockedDamage;
        }
    }
    public override async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.PlayerCombatState != null && player == Owner && Owner.PlayerCombatState.TurnNumber <= 1)
        {
            DynamicVars["HpLost"].BaseValue = 0;
        }
    }
}
