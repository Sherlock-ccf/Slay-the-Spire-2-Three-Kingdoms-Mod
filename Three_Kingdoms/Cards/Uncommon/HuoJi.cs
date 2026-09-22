using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Three_Kingdoms.Character;
using Three_Kingdoms.KeyWords;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class HuoJi : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(HuoJi)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private decimal _extraDamageFromPlays;
    private decimal ExtraDamageFromPlays
    {
        get
        {
            return _extraDamageFromPlays;
        }
        set
        {
            AssertMutable();
            _extraDamageFromPlays = value;
        }
    }
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AllEnemies;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(HuoJi)}.png";
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { TkKeywords.Fire };
    public HuoJi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6m, ValueProp.Move),
        new DynamicVar("Increase", 2m)
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (CombatState == null)
        {
            return;
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
        .FromCard(this)
        .TargetingAllOpponents(CombatState)
        .Execute(choiceContext);
        DynamicVars.Damage.BaseValue += DynamicVars["Increase"].BaseValue;
        ExtraDamageFromPlays += DynamicVars["Increase"].BaseValue;
    }
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DynamicVars.Damage.BaseValue += ExtraDamageFromPlays;
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["Increase"].UpgradeValueBy(1);
    }
}
