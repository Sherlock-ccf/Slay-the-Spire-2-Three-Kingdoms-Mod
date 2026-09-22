using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class QiaoShi : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(QiaoShi)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(QiaoShi)}.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<IntangiblePower>(1m),
        new PowerVar<WeakPower>(3m)
    ];
    public QiaoShi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await PowerCmd.Apply<IntangiblePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(choiceContext, Owner.Creature, DynamicVars.Weak.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
