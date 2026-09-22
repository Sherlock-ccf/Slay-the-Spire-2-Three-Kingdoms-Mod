using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;
using Three_Kingdoms.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class TieSuoLianHuan : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(TieSuoLianHuan)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AllEnemies;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(TieSuoLianHuan)}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> {
        HoverTipFactory.FromPower<LianHuanPower>(),
    };
    public TieSuoLianHuan() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (CombatState == null)
        {
            return;
        }
        foreach (Creature creature in CombatState.Enemies)
        {
            await PowerCmd.Apply<LianHuanPower>(choiceContext, creature, 1, Owner.Creature, this);
        }
        if (IsUpgraded)
        {
            await CardPileCmd.Draw(choiceContext, 1, Owner);
        }
    }
}
