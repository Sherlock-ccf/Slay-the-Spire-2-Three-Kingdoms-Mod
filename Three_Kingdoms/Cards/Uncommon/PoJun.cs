using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class PoJun : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(PoJun)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AnyEnemy;
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DynamicVar("WeakGive",1m),
        new CalculationBaseVar(6m),
        new ExtraDamageVar(2m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel _, Creature? target) => target?.GetPowerAmount<WeakPower>() ?? 0)
    };
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromPower<WeakPower>() };

    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(PoJun)}.png";
    public PoJun() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars["WeakGive"].BaseValue, Owner.Creature, this);
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["WeakGive"].UpgradeValueBy(1m);
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}
