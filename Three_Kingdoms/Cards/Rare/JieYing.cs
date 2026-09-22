using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Three_Kingdoms.Character;
using Three_Kingdoms.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class JieYing : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(JieYing)}.mp3";
    private const int energyCost = 2;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(JieYing)}.png";
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<YingPower>(),
        HoverTipFactory.FromPower<JieYingPower>()
    };
    public JieYing() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (cardPlay.Target == null)
        {
            return;
        }
        if (Owner.HasPower<JieYingPower>())
        {
            await PowerCmd.Remove<JieYingPower>(Owner.Creature);
        }
        await PowerCmd.Apply<JieYingPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, 2, Owner.Creature, this);
        await PowerCmd.Apply<YingPower>(choiceContext, cardPlay.Target, 1, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
