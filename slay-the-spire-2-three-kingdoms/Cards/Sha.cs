using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using slay_the_spire_2_three_kingdoms.Character;
using slay_the_spire_2_three_kingdoms.Node;
using slay_the_spire_2_three_kingdoms.Relics;
namespace slay_the_spire_2_three_kingdoms.Cards;

// 加入哪个卡池
[Pool(typeof(TkCardPool))]
public class Sha : CustomCardModel
{
    public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(Sha)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Basic;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;

    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(Sha)}.png";

    public Sha() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null)
        {
            return;
        }
        CardPlayer.PlayCardSfx(SfxPath);
        FangTianHuaJi? HasF = Owner.Relics.OfType<FangTianHuaJi>().FirstOrDefault();
        if (cardPlay.Target != null && HasF != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(CombatState)
            .Execute(choiceContext);
        }
        else if (cardPlay.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
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
