using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Three_Kingdoms.KeyWords;
using Three_Kingdoms.Character;
using Three_Kingdoms.Powers;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class CuiKe : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(CuiKe)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(7m, ValueProp.Move)
    ];
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(CuiKe)}.png";
    public CuiKe() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { TkKeywords.Fire };
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (cardPlay.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
            await PowerCmd.Apply<LianHuanPower>(choiceContext, cardPlay.Target, 1, Owner.Creature, this);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
