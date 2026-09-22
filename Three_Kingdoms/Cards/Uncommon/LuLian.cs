using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;
using Three_Kingdoms.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Three_Kingdoms.KeyWords;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class LuLian : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(LuLian)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AllEnemies;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(LuLian)}.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6m, ValueProp.Move)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> {
        HoverTipFactory.FromPower<LianHuanPower>(),
    };
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { TkKeywords.Fire };
    public LuLian() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (CombatState == null)
        {
            return;
        }
        Creature target = CombatState.HittableEnemies[0];
        foreach (Creature creature in CombatState.Enemies)
        {
            await PowerCmd.Apply<LianHuanPower>(choiceContext, creature, 1, Owner.Creature, this);
            if (creature.CurrentHp > target.CurrentHp)
            {
                target = creature;
            }
        }
        await CardPileCmd.Draw(choiceContext, 1, Owner);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(target)
            .Execute(choiceContext);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
