using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using slay_the_spire_2_three_kingdoms.Character;
using slay_the_spire_2_three_kingdoms.Node;
namespace slay_the_spire_2_three_kingdoms.Cards;

[Pool(typeof(TkCardPool))]
public class HanZhan : CustomCardModel
{
	public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(HanZhan)}.mp3";
    protected override bool HasEnergyCostX => true;
    private const int energyCost = 0;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move)];
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(HanZhan)}.png";
    public HanZhan() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        int num = ResolveEnergyXValue();
        if (cardPlay.Target != null)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, num, Owner.Creature, this);
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, num, Owner.Creature, this);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(num)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        }

    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}
