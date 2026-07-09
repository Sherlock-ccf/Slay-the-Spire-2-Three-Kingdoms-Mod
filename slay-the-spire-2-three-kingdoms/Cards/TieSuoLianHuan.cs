using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using slay_the_spire_2_three_kingdoms.Character;
using slay_the_spire_2_three_kingdoms.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using slay_the_spire_2_three_kingdoms.Node;
namespace slay_the_spire_2_three_kingdoms.Cards;

// 加入哪个卡池
[Pool(typeof(TkCardPool))]
public class TieSuoLianHuan : CustomCardModel
{
	public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(TieSuoLianHuan)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AllEnemies;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(TieSuoLianHuan)}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> {
        HoverTipFactory.FromPower<LianHuanPower>(),
    };
    public TieSuoLianHuan() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    // 打出时的效果逻辑
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
