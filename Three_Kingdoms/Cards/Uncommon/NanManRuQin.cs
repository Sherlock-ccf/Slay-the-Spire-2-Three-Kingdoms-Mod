using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Creatures;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class NanManRuQin : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(NanManRuQin)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AllEnemies;
    private const bool shouldShowInCardLibrary = true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(13m, ValueProp.Move)
    ];

    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(NanManRuQin)}.png";

    public NanManRuQin() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if(CombatState==null)
        {
            return;
        }
        foreach (Creature enermy in CombatState.HittableEnemies)
        {
            if(enermy.Monster?.IntendsToAttack==false)
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(enermy)
                .Execute(choiceContext);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
