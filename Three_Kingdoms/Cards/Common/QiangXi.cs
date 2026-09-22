using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.ValueProps;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Common;

[Pool(typeof(TkCardPool))]
public class QiangXi : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(QiangXi)}.mp3";

    private const int energyCost = 0;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AnyEnemy;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(9m, ValueProp.Move)
    ];
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(QiangXi)}.png";
    public QiangXi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
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
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
