using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.HoverTips;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class LeBuSiShu : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(LeBuSiShu)}.mp3";
    private const int energyCost = 3;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [StunIntent.GetStaticHoverTip()];

    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(LeBuSiShu)}.png";

    public LeBuSiShu() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if(cardPlay.Target!=null)
        {
            await CreatureCmd.Stun(cardPlay.Target);
        }
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
